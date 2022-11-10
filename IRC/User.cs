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
        string Username { get; set; }

        public User(IPAddress ipAddr, int port, string username) : base(ipAddr)
        {
            Username = username;
        }
    }
}
