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
        Socket sender;

        public Client(IPAddress ipAddr) : base(ipAddr)
        {
            sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Handshake()
        {
            try
            {
                // Connect Socket to the remote endpoint using method Connect()
                sender.Connect(localEndPoint);

                Console.WriteLine(
                    "SocketConnect::Client - Socket connected to -> {0} ",
                    sender.RemoteEndPoint.ToString()
                );
            }
            catch (SocketException se)
            {
                //Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketConnect::Client - Unexpected exception : {0}", e.ToString());
            }
        }


        // Send 
        public Message? Send(Message message)
        {
            if (!sender.Connected) return null;

            try
            {
                sender.Send(message.ToBytes());
                sent.Add(message);
                if (sent.Count == cache) sent.Remove(0);
            }
            catch (SocketException se)
            {
                //Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

            // Receive the return message
            Message? receivedMessage = Receive();

            Console.WriteLine("SocketConnect::Client - Message from Server -> {0}",
                  receivedMessage?.ToString()
            );

            return receivedMessage;
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
                //Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }

            // Create the message
            Message message = new Message().FromBytes(bytesReceived);

            InvokeOnMessageReceived(message);
            return message;
        }
    }
}
