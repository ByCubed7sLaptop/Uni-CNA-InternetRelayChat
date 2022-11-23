using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    [Serializable]
    public class UserJoined : SocketConnect.Packet
    {
        public string Username { get; }

        public UserJoined(string username)
        {
            Username = username;
        }
    }
    
    [Serializable]
    public class UserLeft : SocketConnect.Packet
    {
        public string Username { get; }

        public UserLeft(string username)
        {
            Username = username;
        }

    }
}
