using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    // Represents a chatroom
    public class Chatroom
    {
        public SortedDictionary<Guid, string> Users { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public Chatroom()
        {
            Users = new SortedDictionary<Guid, string>();
            Messages = new List<ChatMessage>();
        }
    }
}
