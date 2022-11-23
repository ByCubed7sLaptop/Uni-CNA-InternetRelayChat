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


        public User(IPAddress ipAddr, int port, string username) : base(ipAddr, port)
        {
            Username = username;
        }

        public void Handshake()
        {
            // TODO: Add id / encryption keys / ect ect
            Send(new IRC.Handshake(Username, null));
        }

        public void SendMessage(string message)
        {
            Send(new IRC.ChatMessage(Username, message));
        }

        public void Disconnect()
        {
            Send(new SocketConnect.Disconnect());
        }
    }
}
