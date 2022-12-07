using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    /// <summary>
    /// Represents a connectable server.
    /// </summary>
    public class Server : Connector
    {
        private const bool DEBUG = false;
        
        /// <summary>
        /// The listening socket.
        /// </summary>
        protected Socket listener;

        /// <summary>
        /// The sockets connected to this server.
        /// </summary>
        protected SynchronizedCollection<Socket> clients;

        /// <summary>
        /// Whether this server is attempting to shutdown.
        /// </summary>
        protected bool isShuttingDown;

        public Server(IPAddress ipAddr, int port) : base(ipAddr, port)
        {
            listener = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            clients = new SynchronizedCollection<Socket>();

            isShuttingDown = false;
        }

        /// <summary>
        /// Start listening for clients to connect.
        /// </summary>
        public void Run()
        {
            isShuttingDown = false;

            try
            {
                // Using Bind() method we associate a network address to the Server Socket
                // All client that will connect to this Server Socket must know this network
                // Address
                listener.Bind(localEndPoint);

                // Using Listen() method we create the Client list that will want
                // to connect to Server
                listener.Listen(10);

                while (true)
                {
                    if (DEBUG) Console.WriteLine("SocketConnect::Server - Waiting connection ... ");

                    // Suspend while waiting for incoming connection
                    Socket clientSocket = listener.Accept();
                    if (DEBUG) Console.WriteLine("SocketConnect::Server - Connected with a client");

                    //ConnectClient(clientSocket);
                    ConnectClientThread(clientSocket).Start();

                    if (isShuttingDown) break;
                }
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Server - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                if (DEBUG) Console.WriteLine(e.ToString());
            }

            TryDisconnectAll();
            clients.Clear();
        }

        public Thread RunThread()
        {
            return new(() => {
                Run();
            });
        }


        protected Thread ConnectClientThread(Socket clientSocket)
        {
            return new Thread(() => ConnectClient(clientSocket));
        }

        /// <summary>
        /// Connect client protocal.
        /// </summary>
        protected void ConnectClient(Socket clientSocket)
        {
            // Add client to list
            clients.Add(clientSocket);
            InvokeOnConnect(clientSocket, ipAddress);

            try
            {
                while (true)
                {
                    // The recieved memory stream
                    MemoryStream stream = new MemoryStream();

                    // The received memory buffer
                    byte[] buffer = new byte[1024];

                    // Recieve!
                    int numByte = clientSocket.Receive(buffer);

                    // If recieved nothing, retry
                    if (numByte == 0) continue;

                    // Write to memory buffer
                    stream.Write(buffer, 0, numByte);

                    // Deserialize packet
                    Packet packet = Packet.FromBytes<Packet>(buffer.ToArray());

                    if (DEBUG) Console.WriteLine("SocketConnect::Server - Text received -> {0} ", packet.ToString());

                    // Send the recieved message
                    //Send(clientSocket, new Recieved());

                    // If the client has told us to shutdown
                    if (packet is Shutdown)
                    {
                        Shutdown();
                        break;
                    }

                    // If the client has told us to disconnect
                    if (packet is Disconnect)
                        break;

                    // Invoke the recieve event
                    InvokeOnPacketReceived(clientSocket, ipAddress, packet);

                    // If we're shutting down
                    if (isShuttingDown) break;
                }
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Server - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Server - Unexpected exception : {0}", e.ToString());
            }            

            // Close client Socket
            TryDisconnect(clientSocket);
            clients.Remove(clientSocket);
        }

        /// <summary>
        /// Send the packet to a specific client.
        /// </summary>
        static public void Send(Socket clientSocket, Packet message)
        {
            Thread.Sleep(100);

            // Send a message to Client
            clientSocket.Send(message.ToBytes());
        }

        /// <summary>
        /// Send the packet to every connected client.
        /// </summary>
        public void Broadcast(Packet message)
        {
            for (int i = 0; i < clients.Count; i++)
                Send(clients[i], message);
        }

        /// <summary>
        /// Shutdown the server.
        /// </summary>
        public void Shutdown()
        {
            if (isShuttingDown) return; // Already shutting down
            if (DEBUG) Console.WriteLine("SocketConnect::Server - Shutting down");
            isShuttingDown = true;
            listener.Close();

            TryDisconnectAll();
            clients.Clear();
        }

        /// <summary>
        /// Attempt to disconnect a client.
        /// </summary>
        protected void TryDisconnect(Socket clientSocket)
        {
            if (!clientSocket.Connected) return;
            if (DEBUG) Console.WriteLine("SocketConnect::Server - Disconnected with a client");

            InvokeOnDisconnect(clientSocket, ipAddress);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        /// <summary>
        /// Attempt to disconnect all of the clients.
        /// </summary>
        protected void TryDisconnectAll()
        {
            for (int i = 0; i < clients.Count; i++)
                TryDisconnect(clients[i]);
        }
    }



    [Serializable] public class Recieved : Packet {}
    [Serializable] public class Shutdown : Packet {}
    [Serializable] public class Disconnect : Packet {}
}
