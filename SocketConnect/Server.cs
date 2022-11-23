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
    public class Server : Connector
    {
        private const bool DEBUG = true;
        
        protected Socket listener;
        protected SynchronizedCollection<Socket> clients;
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

        public void Start()
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


        protected Thread ConnectClientThread(Socket clientSocket)
        {
            return new Thread(() => ConnectClient(clientSocket));
        }
        protected void ConnectClient(Socket clientSocket)
        {
            // Add client to list
            clients.Add(clientSocket);
            InvokeOnConnect(clientSocket, listener);

            try
            {
                while (true)
                {
                    // Data buffer

                    // Recieve data
                    MemoryStream stream = new MemoryStream();
                    byte[] buffer = new byte[1024];
                    //while (true)
                    //{
                    //    int numByte = clientSocket.Receive(buffer);
                    //    if (numByte == 0) break;
                    //    stream.Write(buffer, 0, numByte);
                    //}
                    int numByte = clientSocket.Receive(buffer);
                    stream.Write(buffer, 0, numByte);

                    if (numByte == 0) continue;

                    Packet message = Packet.FromBytes<Packet>(buffer.ToArray());

                    if (DEBUG) Console.WriteLine("SocketConnect::Server - Text received -> {0} ", message.ToString());

                    Send(clientSocket, new Recieved());

                    if (message is Shutdown)
                    {
                        Shutdown();
                        break;
                    }

                    if (message is Disconnect)
                        break;

                    InvokeOnMessageReceived(clientSocket, message);

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

        static public void Send(Socket clientSocket, Packet message)
        {
            // Send a message to Client
            clientSocket.Send(message.ToBytes());
        }

        public void Broadcast(Packet message)
        {
            for (int i = 0; i < clients.Count; i++)
                Send(clients[i], message);
        }

        public void Shutdown()
        {
            if (isShuttingDown) return; // Already shutting down
            if (DEBUG) Console.WriteLine("SocketConnect::Server - Shutting down");
            isShuttingDown = true;
            listener.Close();

            TryDisconnectAll();
            clients.Clear();
        }

        protected void TryDisconnect(Socket clientSocket)
        {
            if (!clientSocket.Connected) return;
            if (DEBUG) Console.WriteLine("SocketConnect::Server - Disconnected with a client");

            InvokeOnDisconnect(clientSocket, listener);

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        protected void TryDisconnectAll()
        {
            for (int i = 0; i < clients.Count; i++)
                TryDisconnect(clients[i]);
        }
    }



    [Serializable]
    public class Recieved : Packet
    {

    }
    [Serializable]
    public class Shutdown : Packet
    {

    }
    [Serializable]
    public class Disconnect : Packet
    {

    }
}
