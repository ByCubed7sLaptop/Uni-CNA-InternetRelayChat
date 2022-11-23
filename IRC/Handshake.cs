using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketConnect;

namespace IRC
{
    [Serializable]
    public class Handshake : Packet
    {
        public Guid? Guid { get; }
        public string Username { get; }

        public Handshake(string username, Guid? guid)
        {
            Username = username;
            Guid = guid;
        }

    }
}
