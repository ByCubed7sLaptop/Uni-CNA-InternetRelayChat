﻿using System;
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
        private const bool DEBUG = false;

        Socket listener;

        SynchronizedCollection<Socket> clients;

        bool isShuttingDown;

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

                    if (DEBUG) Console.WriteLine("SocketConnect::Server - Text received -> {0} ", message.ToString());

                    Send(clientSocket, Message.CreateRecieved(message));

                    if (message.Header == "Shutdown")
                    {
                        Shutdown();
                        break;
                    }

                    if (message.Header == "Disconnect")
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

        static public void Send(Socket clientSocket, Message message)
        {
            // Send a message to Client
            clientSocket.Send(message.ToBytes());
        }

        public void Broadcast(Message message)
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
}
