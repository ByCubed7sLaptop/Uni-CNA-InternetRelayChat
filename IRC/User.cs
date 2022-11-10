using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public class User : SocketConnect.Client
    {
        string Username { get; set; }

        public User(string username)
        {
            Username = username;
        }
    }
}
