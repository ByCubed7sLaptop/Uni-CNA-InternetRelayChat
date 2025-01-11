using System.Net;
using System.Net.Sockets;

namespace ConcurrentNetworkApplications
{
    public class ServerApplication
    {
        public IRC.Chatroom chatroom;
        public SocketConnect.Server server;

        public Dictionary<Socket, Guid> socketToId;

        public ChessGameCollectionManager chessGameCollectionManager;

        public ServerApplication(IPAddress ipAddr, int port)
        {
            chatroom = new IRC.Chatroom();
            server = new SocketConnect.Server(ipAddr, port);

            socketToId = new Dictionary<Socket, Guid>();

            chessGameCollectionManager = new ChessGameCollectionManager();  

            Hook();

            //CreateRunThread().Start();
        }

        public void Run()
        {
            server.RunThread().Start();
        }

        private void Hook()
        {
            // Hook into events
            server.OnConnect += (s, e) => {

            };

            server.OnDisconnect += (s, e) =>
            {
                string username = chatroom.Users[socketToId[e.Client]];
                IRC.UserLeft messageLeft = new IRC.UserLeft(username);
                server.Broadcast(messageLeft);
            };
            
            server.OnPacketReceived += (s, e) => {
                SocketConnect.Packet message = e.Packet;

                // HANDSHAKE
                if (message is IRC.Handshake handshake)
                {
                    // Add user and get the user key
                    Guid guid = chatroom.Add(handshake.Username);
                    socketToId.Add(e.Client, guid);

                    // Send the handshake with the new GUID
                    handshake.Guid = Guid.NewGuid();
                    SocketConnect.Server.Send(e.Client, handshake);

                    // Broadcast userjoined message
                    IRC.UserJoined messageJoined = new IRC.UserJoined(handshake.Username);
                    server.Broadcast(messageJoined);

                    // Send the user update data
                    IRC.UserCollection userCollection = chatroom.UsersPacket();
                    server.Broadcast(userCollection);

                    // Send the message update data
                    IRC.MessageCollection messageCollection = chatroom.MessagePacket();
                    server.Broadcast(messageCollection);
                }

                // PRIVATE MESSAGE
                else if (message is IRC.ChatPrivateMessage chatPrivateMessage)
                {
                    chatroom.Messages.Add(chatPrivateMessage);

                    // Get target guid from name
                    Guid targetID = Guid.Empty;
                    foreach (KeyValuePair<Guid, string> pair in chatroom.Users)
                        if (pair.Value == chatPrivateMessage.Target)
                            targetID = pair.Key;

                    // Get target socket from guid
                    Socket? targetSocket = null;
                    foreach (KeyValuePair<Socket, Guid> pair in socketToId)
                        if (pair.Value == targetID)
                            targetSocket = pair.Key;

                    if (targetSocket == null) return;

                    SocketConnect.Server.Send(targetSocket, chatPrivateMessage);
                    SocketConnect.Server.Send(e.Client, chatPrivateMessage);
                }

                // MESSAGE
                else if (message is IRC.ChatMessage chatMessage)
                {
                    chatroom.Messages.Add(chatMessage);
                    server.Broadcast(message); //?
                }

                else if (message is ChessGameUpdate chessGameUpdate)
                {
                    chessGameCollectionManager.HandlePacket(chessGameUpdate);
                    // Reflect the packet back to both players
                    // TODO: Change socketToId to a doublelinked dictionary
                    foreach (KeyValuePair<Socket, Guid> pair in socketToId)
                    {
                        // Guid to Socket reverse lookup
                        if (pair.Value == chessGameUpdate.Player1) pair.Key.Send(chessGameUpdate.ToBytes());
                        if (pair.Value == chessGameUpdate.Player2) pair.Key.Send(chessGameUpdate.ToBytes());
                    }
                }

                else
                {
                    Console.WriteLine("Server recieved unimplemented packet: " + message.GetType().FullName);
                }
            };
        }
    }
}
