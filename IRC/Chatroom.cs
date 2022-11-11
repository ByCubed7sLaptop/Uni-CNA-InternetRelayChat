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
        public List<User> Users { get; set; }
        public List<ChatMessage> Messages { get; set; }

        public Chatroom()
        {
            Users = new List<User>();
            Messages = new List<ChatMessage>();
        }

        public void _OnConnect(SocketConnect.Server server, SocketConnect.Server.ClientEventArgs e)
        {
            //Console.WriteLine("Chatroom::Chatroom_OnConnect User Connected");
            //Console.WriteLine(" User Connected");
            server.Broadcast(new ChatMessage("Server", "Cube Joined the Chatroom!"));
        }

        public void _OnMessageReceived(SocketConnect.Server server, SocketConnect.Server.MessageEventArgs e)
        {
            SocketConnect.Message message = e.Message;
            //Console.WriteLine("Chatroom::Chatroom_OnMessageReceived " + message.Header);
            
            // Do action based on message header

            if (message is null)
            {
                return;
            }

            // MESSAGE
            else if (message.Header == "ChatMessage")
            {
                string author = message.Args[0];
                string contents = message.Args[1];

                Messages.Add(new ChatMessage(author, contents));
                server.Broadcast(message);
                //Console.WriteLine("Chatroom::Chatroom_OnChatMessageSent " + author + ": " + contents);
                Console.WriteLine(author + ": " + contents);
            }

        }
    }
}
