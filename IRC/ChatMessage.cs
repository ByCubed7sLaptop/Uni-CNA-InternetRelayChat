using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IRC
{
    public class ChatMessage : SocketConnect.Message
    {
        public string Author { get; set; }
        public string Contents { get; set; }
        public DateTime SentAt { get; set; }

        public ChatMessage(string author, string contents)
        {
            Header = "ChatMessage";
            Author = author;
            Contents = contents;
        }

        public override string ToString()
        {
            Args = new List<string>();
            Args.Add(Author);
            Args.Add(Contents);
            Args.Add(SentAt.ToUniversalTime().ToString("O"));

            return base.ToString();
        }

        public override SocketConnect.Message FromString(string data)
        {
            SocketConnect.Message message = base.FromString(data);

            Author = Args[0];
            Contents = Args[1];
            SentAt = DateTime.ParseExact(Args[2], "O", CultureInfo.InvariantCulture);

            return message;
        }

    }
}
