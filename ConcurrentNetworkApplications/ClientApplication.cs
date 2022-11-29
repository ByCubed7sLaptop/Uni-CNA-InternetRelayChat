using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentNetworkApplications
{
    public class ClientApplication
    {
        public IRC.User user;
        public GUI.ChatRoom window;
        public IRC.Chatroom chatroom;

        public ClientApplication(IPAddress ipAddr, int port)
        {
            window = new();
            chatroom = new IRC.Chatroom();
            user = new IRC.User(ipAddr, port, "Ethan");

            HookWindowEvents();
            HookUserEvents();
        }

        /// <summary>
        /// Run the operation threads.
        /// </summary>
        public void Run()
        {
            user.Connect();
            user.ReceiveThread().Start();
            user.SendHandshake();
        }

        private void HookWindowEvents()
        {
            // Link window events to user / chatroom creation
            window.OnSendMessage += (s, e) => user.SendMessage(e.Message);
            window.OnLeave += (s, e) => user.SendDisconnect();
        }

        private void HookUserEvents()
        {
            // Link IRC events
            user.OnConnect += (s, e) => {
                window.AddMessage("Connected successfully");
            };

            user.OnPacketReceived += (s, e) => {

                SocketConnect.Packet message = e.Packet;

                // ChatMessage
                if (message is IRC.ChatMessage chatMessage)
                    window.AddMessage(chatMessage.Author + ": " + chatMessage.Contents);

                // UserJoined
                else if (message is IRC.UserJoined userJoined)
                    window.AddMessage(userJoined.Username + " Joined!");

                // UserLeft
                else if (message is IRC.UserLeft userLeft)
                    window.AddMessage(userLeft.Username + " Left!");

                // UserCollection update
                else if (message is IRC.UserCollection userCollection)
                    chatroom.SetUserCollection(userCollection);

                // Handshake
                else if (message is IRC.Handshake handshake)
                {
                    window.AddMessage("Your server member id is: " + handshake.Guid);
                    user.HandShake(handshake);
                }
            };
        }


        /// <summary>
        /// Show Dialog MUST be called on the main thread
        /// </summary>
        public void ShowDialog()
        {
            window.ShowDialog();
        }
    }
}
