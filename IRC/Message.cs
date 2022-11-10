using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    public class Message : SocketConnect.Message
    {
        public string Author { get; set; }
        public string Contents { get; set; }
        public DateTime SentAt { get; set; }

        public override string ToString()
        {
            Header = "Message";

            Args = new List<string>();
            Args.Add(Author);
            Args.Add(Contents);
            Args.Add(SentAt.ToUniversalTime().ToString("O"));

            return base.ToString();
        }

        public override SocketConnect.Message FromString(string data)
        {
            SocketConnect.Message message = base.FromString(data);

            Header = "Message";
            Author = Args[0];
            Contents = Args[1];
            SentAt = DateTime.ParseExact(Args[2], "O", CultureInfo.InvariantCulture);

            return message;
        }

    }
}
