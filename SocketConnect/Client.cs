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

        //List<Packet> awaitingResponses;

        public Client(IPAddress ipAddr, int port) : base(ipAddr, port)
        {
            sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            //awaitingResponses = new List<Packet>();
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
        public void Send(Packet message)
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
            //Packet? receivedPacket = Receive();

            //if (DEBUG) Console.WriteLine("SocketConnect::Client - Packet from Server -> {0}",
            //      receivedPacket?.ToString()
            //);

            //return receivedPacket;
        }

        public Packet? Receive()
        {
            if (!sender.Connected) return null;

            // Data buffer
            byte[] bytesReceived = new byte[1024];

            // Receive the bytes
            int amount;
            try
            {
                amount = sender.Receive(bytesReceived);
            }
            catch (SocketException e)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - SocketException : {0}", e.ToString());
                return null;
            }
            catch (Exception e)
            {
                if (DEBUG) Console.WriteLine(e.ToString());
                return null;
            }

            if (amount == 0) return null;

            // Create the message
            Packet message = Packet.FromBytes<Packet>(bytesReceived);

            if (DEBUG) Console.WriteLine("SocketConnect::Client - Recieved : {0}", message?.ToString());

            //if (message is not null) 
                //InvokeOnPacketReceived(sender, message);

            return message;
        }

        public Thread ReceiveThread()
        {
            return new Thread(() =>
            {
                while (true)
                {
                    Packet? message = Receive();
                    if (message is null) break;

                }
            });
        }

    }
}
