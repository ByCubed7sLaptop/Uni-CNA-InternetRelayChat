using System;
using System.Collections.Generic;
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

        public Client() : base()
        {
            sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );
        }

        public void Ready()
        {
            try
            {
                // Connect Socket to the remote endpoint using method Connect()
                sender.Connect(localEndPoint);

                Console.WriteLine(
                    "SocketConnect::Client - Socket connected to -> {0} ",
                    sender.RemoteEndPoint.ToString()
                );

                Send(new Message("Message#A"));
                Send(new Message("Message#B"));
                Send(Message.CreateShutdown());
                Send(new Message("Message#C"));

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("SocketConnect::Client - ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketConnect::Client - SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("SocketConnect::Client - Unexpected exception : {0}", e.ToString());
            }
        }


        // Send 
        public Message Send(Message message)
        {
            byte[] messageToSend = message.ToBytes();
            sender.Send(messageToSend);

            // Receive the return message
            Message receivedMessage = Receive();

            Console.WriteLine("SocketConnect::Client - Message from Server -> {0}",
                  receivedMessage.ToString()
            );

            return receivedMessage;
        }

        public Message Receive()
        {
            // Data buffer
            byte[] bytesReceived = new byte[1024];

            // Receive the bytes
            sender.Receive(bytesReceived);

            // Create the message
            Message message = new Message().FromBytes(bytesReceived);

            InvokeOnMessageReceived(message);
            return message;
        }

    }
}
