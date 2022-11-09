using System;
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
        Socket listener;

        HashSet<Socket> clients;

        public Server() : base()
        {
            listener = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            clients = new HashSet<Socket>();
        }

        public void Ready() 
        {
            StartListening();
        }

        public void StartListening()
        {
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
                    Console.WriteLine("SocketConnect::Server - Waiting connection ... ");

                    // Suspend while waiting for incoming connection
                    Socket clientSocket = listener.Accept();

                    // TODO: Send to seperate thread
                    ConnectClient(clientSocket);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void ConnectClient(Socket clientSocket)
        {
            // Add client to list
            clients.Add(clientSocket);
            InvokeOnConnect();

            while (true)
            {
                // Data buffer
                byte[] bytes = new Byte[1024];
                string data = null;

                // Recieve data
                while (true)
                {
                    int numByte = clientSocket.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, numByte);
                    if (data.IndexOf(EOF) > -1) break;
                }

                Message message = new Message().FromString(data);

                Console.WriteLine("SocketConnect::Server - Text received -> {0} ", message.ToString());
                
                Send(clientSocket, Message.CreateRecieved());

                if (message.Header == "Shutdown")
                    break;
            }
            
            // Close client Socket
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();

            // Remove client from list
            clients.Remove(clientSocket);
            InvokeOnDisconnect();
        }


        public void Send(Socket clientSocket, Message message)
        {
            // Send a message to Client
            clientSocket.Send(message.ToBytes());
        }

    }
}
