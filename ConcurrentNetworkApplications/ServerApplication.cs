using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentNetworkApplications
{
    public class ServerApplication
    {
        public IRC.Chatroom chatroom;
        public SocketConnect.Server server;


        public ServerApplication(IPAddress ipAddr, int port)
        {
            chatroom = new IRC.Chatroom();
            server = new SocketConnect.Server(ipAddr, port);

            Hook();

            //CreateRunThread().Start();
        }

        private void Hook()
        {
            // Hook into events
            //server.OnConnect += (s, e) => { server.Broadcast(new IRC.ChatMessage("Server", "Cube Joined the Chatroom!")); };
            server.OnPacketReceived += (s, e) => {
                SocketConnect.Packet message = e.Packet;
                //Console.WriteLine("Chatroom::Chatroom_OnMessageReceived " + message);

                // HANDSHAKE
                if (message is IRC.Handshake handshake)
                {
                    // Generate a user key
                    Guid guid = Guid.NewGuid();

                    // Add user
                    chatroom.Users.Add(guid, handshake.Username);

                    // Send the handshake with the new GUID
                    handshake.Guid = Guid.NewGuid();
                    SocketConnect.Server.Send(e.Client, handshake);

                    // Broadcast userjoined message
                    IRC.UserJoined messageJoined = new IRC.UserJoined(handshake.Username);

                    server.Broadcast(messageJoined);
                }

                // MESSAGE
                else if (message is IRC.ChatMessage chatMessage)
                {
                    chatroom.Messages.Add(chatMessage);
                    server.Broadcast(message); //?
                }

                else
                {
                    Console.WriteLine("Server: " + message);
                }
            };
        }

        public Thread CreateRunThread()
        {
            return new(() => {
                server.Start();

                Thread.Sleep(10000);
                server.Shutdown();
                Console.WriteLine("Server Shutdown");
                //serverThread.Join();
            });
        }
    }
}
