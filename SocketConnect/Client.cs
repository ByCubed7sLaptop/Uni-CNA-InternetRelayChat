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
    /// <summary>
    /// Represents a client connecting to a server.
    /// </summary>
    public class Client : Connector
    {
        private const bool DEBUG = false;

        /// <summary>
        /// The socket used in client operations.
        /// </summary>
        protected readonly Socket sender;

        public Client(IPAddress ipAddr, int port) : base(ipAddr, port)
        {
            sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        /// <summary>
        /// Connect to the remote end point.
        /// </summary>
        public void Connect()
        {
            try
            {
                // Connect the Socket using method Connect()
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


        /// <summary>
        /// Send a packet to the server.
        /// </summary>
        public bool Send(Packet message)
        {
            // Make sure we're connected
            if (!sender.Connected) return false;

            // Give the client and server some breathing room
            Thread.Sleep(100);

            try
            {
                // Send the packet
                sender.Send(message.ToBytes());
                //awaitingResponses.Add(message);
                return true; 
            }
            catch (SocketException se)
            {
                if (DEBUG) Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }

        /// <summary>
        /// Attempt to recieve a packet, if any.
        /// </summary>
        public void Receive()
        {
            while (true)
            {                
                if (!sender.Connected) break;

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
                    break;
                }
                catch (Exception e)
                {
                    if (DEBUG) Console.WriteLine(e.ToString());
                    break;
                }

                if (amount == 0) break;

                // Deserialize the packet
                Packet message = Packet.FromBytes<Packet>(bytesReceived);

                if (DEBUG) Console.WriteLine("SocketConnect::Client - Recieved : {0}", message?.ToString());

                // Raise the packet recieved event
                InvokeOnPacketReceived(sender, ipAddress, message);
            }
        }

        /// <summary>
        /// Create a thread that recieves packets from the server.
        /// </summary>
        public Thread ReceiveThread()
        {
            return new Thread(() => Receive());
        }

    }
}
