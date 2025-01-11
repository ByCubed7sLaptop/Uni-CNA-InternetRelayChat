using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public class User : SocketConnect.Client
    {
        // TODO: Add id / encryption keys / ect ect

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The server id of the user.
        /// </summary>
        public Guid Id { get; set; }

        public User(IPAddress ipAddr, int port, string name) : base(ipAddr, port)
        {
            Name = name;
            Id = Guid.Empty;
        }

        public void HandShake(IRC.Handshake handshake)
        {
            Guid? guid = handshake.Guid;

            if (guid == Guid.Empty)
                throw new Exception("Handshake failed to send a filled GUID!");
            
            Id = guid.Value;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Send packet functions

        public void SendHandshake()
        {
            Send(new Handshake(Name, Guid.Empty));
        }

        public void SendMessage(string message)
        {
            Send(new IRC.ChatMessage(Name, message));
        }

        public void SendPrivateMessage(string target, string message)
        {
            Send(new IRC.ChatPrivateMessage(target, Name, message));
        }

        public void SendDisconnect()
        {
            Send(new SocketConnect.Disconnect());
        }
    }
}
