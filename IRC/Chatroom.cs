using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IRC
{
    /// <summary>
    /// Represents a chatroom, contains a collection of users and messages.
    /// </summary>
    public class Chatroom
    {
        /// <summary>
        /// The current connected users.
        /// </summary>
        public SortedDictionary<Guid, string> Users { get; set; }
        
        /// <summary>
        /// The messages in this chatroom.
        /// </summary>
        public List<ChatMessage> Messages { get; set; }

        public Chatroom()
        {
            Users = new SortedDictionary<Guid, string>();
            Messages = new List<ChatMessage>();
        }

        /// <summary>
        /// Get the user collection as an update packet.
        /// </summary>
        public UserCollection UsersPacket() 
            => new UserCollection(Users);

        /// <summary>
        /// Get the message collection as an update packet.
        /// </summary>
        public UserCollection MessagePacket()
            => new UserCollection(Users);

        /// <summary>
        /// Set the user collection using a packet.
        /// </summary>
        public void SetUserCollection(UserCollection userCollection) 
            => Users = userCollection.Users;

        /// <summary>
        /// Set the message collection using a packet.
        /// </summary>
        public void MessageFromPacket(MessageCollection messageCollection) 
            => Messages = messageCollection.Messages;
    }
}
