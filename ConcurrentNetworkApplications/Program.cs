using System;
using System.Net;
using System.Threading;
using System.Windows;

using CubedChess;
using SocketConnect;

// Put everything together! :D
namespace ConcurrentNetworkApplications
{
    class Program
    {
        static readonly IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        static readonly IPAddress ipAddr = ipHost.AddressList[0];
        static readonly int port = 11111;

        [STAThread]
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, ConcurrentNetworkApplications!");

            //Board board = new Board();

            //board.DebugPrint();
            //Console.WriteLine();

            //board.Move(Board.b2, Board.b3);
            //board.Move(Board.c1, Board.a3);
            //board.Move(Board.a3, Board.b4);

            //board.DebugPrint();
            //Console.WriteLine();

            //board.DebugPrintNumbers();
            //Console.WriteLine();

            CreateServer().Start();

            //(GUI.ChatRoom window, IRC.User user) = CreateClient();
            //(GUI.ChatRoom window2, IRC.User user2) = CreateClient();

            // Show Dialog MUST be called on the main thread
            //window.ShowDialog();
            
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Client

        public static (GUI.ChatRoom, IRC.User) CreateClient()
        {
            GUI.ChatRoom window = new();
            IRC.Chatroom chatroom = new IRC.Chatroom();
            IRC.User user = new IRC.User(ipAddr, port, "Ethan");

            // Link window events to user / chatroom creation
            window.OnSendMessage += (s, e) => user.SendMessage(e.Message);
            window.OnLeave += (s, e) => user.Disconnect();
            

            // Link IRC events
            user.OnConnect += (s, e) => { };
            user.OnPacketReceived += (s, e) => {

                SocketConnect.Packet message = e.Packet;

                if (message is IRC.ChatMessage chatMessage)
                    //Console.WriteLine(user.Username + " recieved: " + chatMessage.Author + ": " + chatMessage.Contents);
                    window.AddMessage(chatMessage.Author + ": " + chatMessage.Contents);

                else if (message is IRC.UserJoined userJoined)
                    window.AddMessage(userJoined.Username + " Joined!");
                //Console.WriteLine(user.Username + " recieved: " + userJoined.Username + " Joined!");

                else if (message is IRC.UserLeft userLeft)
                    window.AddMessage(userLeft.Username + " Left!");
                //Console.WriteLine(user.Username + " recieved: " + userJoined.Username + " Joined!");

                else if (message is IRC.Handshake handshake)
                    window.AddMessage("Your server member id is: " + handshake.Guid);
                    //Console.WriteLine(user.Username + "'s server member id is: " + handshake.Guid);

                else
                    ;// Console.WriteLine(user1.Username + " recieved message: " + message.Id);


            };


            // Link Window events



            // Start User
            new Thread(() => {
                user.Connect();
                user.ReceiveThread().Start();

                user.Handshake();

                Thread.Sleep(1000);

                //user.SendMessage("Sending message");
            }).Start();

            return (window, user);
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Server

        public static Thread CreateServer()
        {
            IRC.Chatroom chatroom = new IRC.Chatroom();
            SocketConnect.Server server = new SocketConnect.Server(ipAddr, port);

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


            Thread thread = new Thread(() => {
                server.Start();

                Thread.Sleep(10000);
                server.Shutdown();
                Console.WriteLine("Server Shutdown");
                //serverThread.Join();
            });

            return thread;
        }




    }
}


