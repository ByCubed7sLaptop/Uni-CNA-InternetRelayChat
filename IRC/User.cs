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
        public string Username { get; set; }
        public Guid Id { get; set; }

        public User(IPAddress ipAddr, int port, string username) : base(ipAddr, port)
        {
            Username = username;
        }

        public void HandShake(IRC.Handshake handshake)
        {
            Guid? guid = handshake.Guid;

            if (!guid.HasValue)
                throw new Exception("Handshake failed to send a valid GUID!");
            
            Id = guid.Value;
        }

        // Send packet functions

        public void SendHandshake()
        {
            // TODO: Add id / encryption keys / ect ect
            Send(new IRC.Handshake(Username, null));
        }

        public void SendMessage(string message)
        {
            Send(new IRC.ChatMessage(Username, message));
        }

        public void SendDisconnect()
        {
            Send(new SocketConnect.Disconnect());
        }
    }
}
