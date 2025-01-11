using System.Net;

namespace ConcurrentNetworkApplications
{
    public class ClientApplication
    {
        public IRC.User user;
        public GUI.ChatRoom window;
        public IRC.Chatroom chatroom;

        public ChessGameCollectionManager chessGameCollectionManager;

        public ClientApplication(IPAddress ipAddr, int port, string name)
        {
            chatroom = new IRC.Chatroom();
            user = new IRC.User(ipAddr, port, name);

            chessGameCollectionManager = new ChessGameCollectionManager();

            // TODO: ChessGameCollectionManager.ChessGame handles update events
            //    Hook into them for updating the UI
        }

        /// <summary>
        /// Run the operation threads.
        /// </summary>
        public void Run()
        {
            HookUserEvents();

            user.Connect();
            user.ReceiveThread().Start();
            user.SendHandshake();
        }

        private void HookWindowEvents()
        {
            // Link window events to user / chatroom creation
            window.OnSendMessage += (s, e) => {
                
                // Is the user trying to send a private message?
                if (!e.Message.StartsWith("/w"))
                {
                    user.SendMessage(e.Message);
                    return;
                }

                List<string> messageSplit = e.Message.Split(' ').ToList();
                user.SendPrivateMessage(
                    messageSplit[1],
                    string.Join(' ', messageSplit.GetRange(2, messageSplit.Count - 2))
                );
            };
            window.OnLeave += (s, e) => user.SendDisconnect();
        }

        private void HookUserEvents()
        {
            user.OnPacketReceived += (s, e) => {

                SocketConnect.Packet message = e.Packet;
                if (message is null) throw new NullReferenceException();

                // Handshake
                else if (message is IRC.Handshake handshake)
                {
                    window?.AddMessage("Your server member id is: " + handshake.Guid);
                    user.HandShake(handshake);
                }

                // UserCollection update
                else if (message is IRC.UserCollection userCollection)
                {
                    chatroom.UserFromPacket(userCollection);
                    window?.UpdateMembers(chatroom.Users.Values.ToList());
                }

                // MessageCollection update
                else if (message is IRC.MessageCollection messageCollection)
                    chatroom.MessageFromPacket(messageCollection);

                // ChatPrivateMessage
                else if (message is IRC.ChatPrivateMessage chatPrivateMessage)
                    window?.AddMessage(chatPrivateMessage.Author + " whispers: " + chatPrivateMessage.Contents);

                // ChatMessage
                else if (message is IRC.ChatMessage chatMessage)
                    window?.AddMessage(chatMessage.Author + ": " + chatMessage.Contents);

                // UserJoined
                else if (message is IRC.UserJoined userJoined)
                    window?.AddMessage(userJoined.Username + " Joined!");

                // UserLeft
                else if (message is IRC.UserLeft userLeft)
                    window?.AddMessage(userLeft.Username + " Left!");

                // Chess Game Update
                // NOTE: Realistically we would move this to a seperate allocated server for chess stuff
                //    But seeing as I do not have the time, I will dump it here for now
                else if (message is ChessGameUpdate chessGameUpdate)
                {
                    chessGameCollectionManager.HandlePacket(chessGameUpdate);

                    // TODO: Update the UI approprietly?
                }

                else Console.WriteLine("Server recieved unimplemented packet: " + message.GetType().FullName);
                
            };
        }

        /// <summary>
        /// Show Dialog MUST be called on the main thread
        /// </summary>
        public void ShowDialog()
        {
            window = new();
            HookWindowEvents();
            window.ShowDialog();
        }
    }
}
