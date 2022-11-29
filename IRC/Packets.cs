using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    [Serializable]
    public class ChatMessage : SocketConnect.Packet
    {
        public string Author { get; set; }
        public string Contents { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now.ToUniversalTime();

        public ChatMessage(string author, string contents)
        {
            Author = author;
            Contents = contents;
        }

        public ChatMessage() : this("NOAUTH", "NOCONT") {}
    }


    [Serializable]
    public class Handshake : SocketConnect.Packet
    {
        public Guid Guid { get; set; }
        public string Username { get; }

        public Handshake(string username, Guid guid)
        {
            Username = username;
            Guid = guid;
        }
    }


    [Serializable]
    public class UserCollection : SocketConnect.Packet
    {
        public SortedDictionary<Guid, string> Users { get; }

        public UserCollection(SortedDictionary<Guid, string> users)
        {
            Users = users;
        }
    }


    [Serializable]
    public class UserJoined : SocketConnect.Packet
    {
        public string Username { get; }

        public UserJoined(string username)
        {
            Username = username;
        }
    }


    [Serializable]
    public class UserLeft : SocketConnect.Packet
    {
        public string Username { get; }

        public UserLeft(string username)
        {
            Username = username;
        }
    }


    [Serializable]
    public class MessageCollection : SocketConnect.Packet
    {
        public List<ChatMessage> Messages { get; }

        public MessageCollection(List<ChatMessage> messages)
        {
            Messages = messages;
        }
    }
}
