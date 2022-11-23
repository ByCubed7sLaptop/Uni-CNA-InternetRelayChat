using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketConnect;


namespace IRC
{
    [Serializable]
    public class ChatMessage : Packet
    {
        public string Author { get; set; }
        public string Contents { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now.ToUniversalTime();

        public ChatMessage()
        {
            Author = "NOAUTH";
            Contents = "NOCONT";
        }
        public ChatMessage(string author, string contents) : base()
        {
            Author = author;
            Contents = contents;
        }

    }
}
