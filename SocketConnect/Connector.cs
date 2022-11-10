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

        protected IPHostEntry ipHost;
        protected IPAddress ipAddr;
        protected IPEndPoint localEndPoint;

        public Connector()
        {
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddr, 11111);
        }

        // When a client connects
        public event EventHandler OnConnect;
        public void InvokeOnConnect()
        {
            EventHandler eventHandler = OnConnect;
            if (eventHandler is null) return;
            eventHandler(this, new EventArgs());
        }

        // When a client disconnects
        public event EventHandler OnDisconnect;
        public void InvokeOnDisconnect()
        {
            EventHandler eventHandler = OnDisconnect;
            if (eventHandler is null) return;
            eventHandler(this, new EventArgs());
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

    }
}


