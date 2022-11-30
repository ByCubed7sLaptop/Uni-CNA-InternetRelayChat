using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentNetworkApplications
{
    public class ServerApplication
    {
        public IRC.Chatroom chatroom;
        public SocketConnect.Server server;

        public Dictionary<Socket, Guid> socketToId;

        public ServerApplication(IPAddress ipAddr, int port)
        {
            chatroom = new IRC.Chatroom();
            server = new SocketConnect.Server(ipAddr, port);

            socketToId = new Dictionary<Socket, Guid>();

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
                    SocketConnect.Server.Send(e.Client, userCollection);

                    // Send the message update data
                    IRC.MessageCollection messageCollection = chatroom.MessagePacket();
                    SocketConnect.Server.Send(e.Client, messageCollection);
                }

                // MESSAGE
                else if (message is IRC.ChatMessage chatMessage)
                {
                    chatroom.Messages.Add(chatMessage);
                    server.Broadcast(message); //?
                }

                else
                {
                    Console.WriteLine("Server recieved unimplemented packet: " + message.GetType().FullName);
                }
            };
        }
    }
}
