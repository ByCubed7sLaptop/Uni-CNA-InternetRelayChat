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
        /// Adds a user to the user list.
        /// </summary>
        /// <param name="username">The user to add.</param>
        /// <returns>The generated Guid of the user.</returns>
        public Guid Add(string username)
        {
            // Generate a user key
            Guid guid = Guid.NewGuid();

            // Add user with given nickname
            Users.Add(guid, username);

            return guid;
        }

        /// <summary>
        /// Get the user collection as an update packet.
        /// </summary>
        public UserCollection UsersPacket()
            => new UserCollection(Users);

        /// <summary>
        /// Get the message collection as an update packet.
        /// </summary>
        public MessageCollection MessagePacket()
            => new MessageCollection(Messages);

        /// <summary>
        /// Set the user collection using a packet.
        /// </summary>
        public void UserFromPacket(UserCollection userCollection)
            => Users = userCollection.Get();

        /// <summary>
        /// Set the message collection using a packet.
        /// </summary>
        public void MessageFromPacket(MessageCollection messageCollection)
            => Messages = messageCollection.Messages.ToList();
    }
}
