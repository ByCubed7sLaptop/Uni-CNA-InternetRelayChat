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
    public class Connector
    {
        //private IPHostEntry ipHost;
        protected IPAddress ipAddress;
        protected IPEndPoint localEndPoint;

        protected const int cache = 5;
        protected Cubed.Collections.Hashlist<Packet> sent;


        public Connector(IPAddress ipAddr, int port = 11111)
        {
            //ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddress = ipAddr;
            localEndPoint = new IPEndPoint(ipAddress, port);

            sent = new Cubed.Collections.Hashlist<Packet>();
        }

        // When a client connects
        public event EventHandler<ClientEventArgs>? OnConnect;
        public void InvokeOnConnect(Socket client, Socket host)
        {
            EventHandler<ClientEventArgs>? eventHandler = OnConnect;
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, host));
        }

        // When a client disconnects
        public event EventHandler<ClientEventArgs>? OnDisconnect;
        public void InvokeOnDisconnect(Socket client, Socket host)
        {
            EventHandler<ClientEventArgs>? eventHandler = OnDisconnect;
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, host));
        }

        // When *I* shutdown
        public event EventHandler? OnShutdown;
        public void InvokeOnShutdown()
        {
            EventHandler? eventHandler = OnShutdown;
            if (eventHandler is null) return;
            eventHandler(this, new EventArgs());
        }

        // When *I* received a message
        public event EventHandler<MessageEventArgs>? OnMessageReceived;
        public void InvokeOnMessageReceived(Socket client, Packet message)
        {
            Debug.Assert(client != null, "Client can not be null!");
            Debug.Assert(message != null, "Message can not be null!");
            EventHandler<MessageEventArgs>? eventHandler = OnMessageReceived;
            if (eventHandler is null) return;
            eventHandler(this, new MessageEventArgs(client, message));
        }

        public class MessageEventArgs : EventArgs
        {
            public Packet Message { get; set; }
            public Socket Client { get; set; }

            public MessageEventArgs(Socket client, Packet message)
            {
                Client = client;
                Message = message;
            }
        }
        public class ClientEventArgs : EventArgs
        {
            public Socket Client { get; set; }
            public Socket Host { get; set; }

            public ClientEventArgs(Socket client, Socket host)
            {
                Client = client;
                Host = host;
            }
        }

    }
}


