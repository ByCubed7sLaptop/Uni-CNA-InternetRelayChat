using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Client : Connector
    {
        private const bool DEBUG = false;

        Socket sender;

        //List<Message> awaitingResponses;

        public Client(IPAddress ipAddr, int port) : base(ipAddr, port)
        {
            sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            //awaitingResponses = new List<Message>();
        }

        public void Connect()
        {
            try
            {
                // Connect Socket to the remote endpoint using method Connect()
                sender.Connect(localEndPoint);

                if (DEBUG) Console.WriteLine("SocketConnect::Client - Socket connected to -> {0} ", sender.RemoteEndPoint.ToString() );
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - Unexpected exception : {0}", e.ToString());
            }
        }


        // Send 
        public void Send(Message message)
        {
            if (!sender.Connected) return;

            try
            {
                sender.Send(message.ToBytes());
                //awaitingResponses.Add(message);
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }

            // Give the client and server some breathing room
            Thread.Sleep(100);

            // Receive the return message
            //Message? receivedMessage = Receive();

            //if (DEBUG) Console.WriteLine("SocketConnect::Client - Message from Server -> {0}",
            //      receivedMessage?.ToString()
            //);

            //return receivedMessage;
        }

        public Message? Receive()
        {
            if (!sender.Connected) return null;

            // Data buffer
            byte[] bytesReceived = new byte[1024];

            // Receive the bytes
            try
            {
                sender.Receive(bytesReceived);
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                if (DEBUG) Console.WriteLine(e.ToString());
                return null;
            }

            // Create the message
            Message message = new Message().FromBytes(bytesReceived);

            //for (int i = 0; i < awaitingResponses.Count; i++)
            //    if (awaitingResponses[i].Equals(message))
            //        awaitingResponses.RemoveAt(i);
                    
            InvokeOnMessageReceived(message);
            return message;
        }

        public Thread ReceiveThread()
        {
            return new Thread(() =>
            {
                while (true)
                {
                    Message message = Receive();
                    if (message is null) break;

                    if (DEBUG) Console.WriteLine("SocketConnect::Client - Recieved : {0}", message.ToString());
                }
            });
        }
    }
}
