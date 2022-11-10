using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    // Represents a chatroom
    public class Chatroom : SocketConnect.Server
    {
        public List<Message> messages = new List<Message>();

        public Message CreateMessage(string from, string contents) => (Message) new Message().Add(from).Add(contents);

        public Chatroom() : base()
        {
            OnConnect += Chatroom_OnConnect;
        }

        private void Chatroom_OnConnect(object? sender, ClientEventArgs e)
        {
            Broadcast(CreateMessage("Cube", "AAAAAAAAAAAAAAA"));
        }
    }
}
