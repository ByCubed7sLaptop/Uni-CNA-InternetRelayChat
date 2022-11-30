using System.Net;
using System.Net.Sockets;

namespace SocketConnect
{
    /// <summary>
    /// Represents a connector that can send packets and connect to another. 
    /// </summary>
    public class Connector
    {
        /// <summary>
        /// The connectors target, stores the end ip address and port.
        /// </summary>
        protected IPEndPoint localEndPoint;
        protected readonly IPAddress ipAddress;

        // Packet cache
        //protected const int cache = 5;
        //protected Cubed.Collections.Hashlist<Packet> sent;

        public Connector(IPAddress ipAddr, int port = 11111)
        {
            //ipHost = Dns.GetHostEntry(Dns.GetHostName());
            //ipAddress = ipAddr;
            localEndPoint = new IPEndPoint(ipAddr, port);
            ipAddress = ipAddr;

            //sent = new Cubed.Collections.Hashlist<Packet>();
        }

        /// <summary>
        /// Invoked when the connector is connecting.
        /// </summary>
        public event EventHandler<ClientEventArgs>? OnConnect;
        public void InvokeOnConnect(Socket client, IPAddress address)
        {
            // Localize handler -> now thread safe
            EventHandler<ClientEventArgs>? eventHandler = OnConnect;

            // Invoke
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, address));
        }

        /// <summary>
        /// Invoked when the connector is disconnecting.
        /// </summary>
        public event EventHandler<ClientEventArgs>? OnDisconnect;
        public void InvokeOnDisconnect(Socket client, IPAddress address)
        {
            // Localize handler -> now thread safe
            EventHandler<ClientEventArgs>? eventHandler = OnDisconnect;

            // Invoke
            if (eventHandler is null) return;
            eventHandler(this, new ClientEventArgs(client, address));
        }

        /// <summary>
        /// Invoked when the connector is shutting down.
        /// </summary>
        public event EventHandler? OnShutdown;
        public void InvokeOnShutdown()
        {
            // Localize handler -> now thread safe
            EventHandler? eventHandler = OnShutdown;

            // Invoke
            if (eventHandler is null) return;
            eventHandler(this, new EventArgs());
        }

        /// <summary>
        /// Invoked when a packet is recieved by the connector.
        /// </summary>
        public event EventHandler<PacketEventArgs>? OnPacketReceived;
        public void InvokeOnPacketReceived(Socket client, IPAddress address, Packet packet)
        {
            // Arguments can't be null
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (address is null) throw new ArgumentNullException(nameof(address));
            if (packet is null) throw new ArgumentNullException(nameof(packet));

            // Localize handler -> now thread safe
            EventHandler<PacketEventArgs>? eventHandler = OnPacketReceived;
            
            // Invoke
            if (eventHandler is null) return;
            eventHandler(this, new PacketEventArgs(client, address, packet));
        }


        public class PacketEventArgs : ClientEventArgs
        {
            /// <summary> The packet that was recieved </summary>
            public Packet Packet { get; set; }

            public PacketEventArgs(Socket client, IPAddress host, Packet packet) 
                : base(client, host)
            {
                Packet = packet;
            }
        }

        public class ClientEventArgs : EventArgs
        {
            /// <summary> The socket that caused it </summary>
            public Socket Client { get; set; }
            /// <summary> The recieving socket </summary>
            public IPAddress Address { get; set; }

            public ClientEventArgs(Socket client, IPAddress address)
            {
                Client = client;
                Address = address;
            }
        }

    }
}


