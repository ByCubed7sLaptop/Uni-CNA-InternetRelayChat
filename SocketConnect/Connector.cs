using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Connector
    {
        public const char EOF = ';';
        public const char Split = '|';

        //private IPHostEntry ipHost;
        protected IPAddress ipAddr;
        protected IPEndPoint localEndPoint;

        protected const int cache = 5;
        protected Cubed.Collections.Hashlist<Message> sent;


        public Connector(IPAddress ipAddr, int port = 11111)
        {
            //ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipAddr;
            localEndPoint = new IPEndPoint(ipAddr, port);

            sent = new Cubed.Collections.Hashlist<Message>();
        }

        // When a client connects
        public event EventHandler<ClientEventArgs> OnConnect;
        public void InvokeOnConnect(Socket client, Socket host)
        {
            EventHandler<ClientEventArgs> eventHandler = OnConnect;
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, host));
        }

        // When a client disconnects
        public event EventHandler<ClientEventArgs> OnDisconnect;
        public void InvokeOnDisconnect(Socket client, Socket host)
        {
            EventHandler<ClientEventArgs> eventHandler = OnDisconnect;
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, host));
        }

        // When *I* shutdown
        public event EventHandler OnShutdown;
        public void InvokeOnShutdown()
        {
            EventHandler eventHandler = OnShutdown;
            if (eventHandler is null) return;
            eventHandler(this, new EventArgs());
        }

        // When *I* received a message
        public event EventHandler<MessageEventArgs> OnMessageReceived;
        public void InvokeOnMessageReceived(Message message)
        {
            EventHandler<MessageEventArgs> eventHandler = OnMessageReceived;
            if (eventHandler is null) return;
            eventHandler(this, new MessageEventArgs(message));
        }

        public class MessageEventArgs : EventArgs
        {
            public Message Message { get; set; }

            public MessageEventArgs(Message message)
            {
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


