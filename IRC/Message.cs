using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public class Message : SocketConnect.Message
    {
        public string Author { get; set; }
        public string Description { get; set; }


    }
}
