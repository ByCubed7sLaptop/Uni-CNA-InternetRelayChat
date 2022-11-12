using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IRC
{
    public class ChatMessage : SocketConnect.IMessage
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

        public string Header => "ChatMessage";
        public List<string> Args()
        {
            List<string> args = new List<string>();
            args.Add(Author);
            args.Add(Contents);
            args.Add(SentAt.ToString("O", CultureInfo.InvariantCulture));
            return args;
        }

        public void FromArgs(List<string> args)
        {
            Author = args[0];
            Contents = args[1];
            SentAt = DateTime.ParseExact(args[2], "O", CultureInfo.InvariantCulture);
        }

    }
}
