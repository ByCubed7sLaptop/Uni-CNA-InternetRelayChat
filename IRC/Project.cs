using SocketConnect;
using System.Net;
using System.Threading;

IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddr = ipHost.AddressList[0];
int port = 11111;

IRC.Chatroom serverChatroom = new IRC.Chatroom();
SocketConnect.Server server = new SocketConnect.Server(ipAddr, port);


IRC.Chatroom user1Chatroom = new IRC.Chatroom();
IRC.User user1 = new IRC.User(ipAddr, port, "Ethan");

IRC.Chatroom user2Chatroom = new IRC.Chatroom();
IRC.User user2 = new IRC.User(ipAddr, port, "Thomas");


Thread serverThread = new Thread(() => {
    // Hook into events
    //server.OnConnect += (s, e) => { server.Broadcast(new IRC.ChatMessage("Server", "Cube Joined the Chatroom!")); };
    server.OnMessageReceived += (s, e) => { 
        SocketConnect.Packet message = e.Message;
        //Console.WriteLine("Chatroom::Chatroom_OnMessageReceived " + message.Header);

        // Do action based on message header
        if (false) ;

        // HANDSHAKE
        else if (message is IRC.Handshake)
        {
            IRC.Handshake handshake = (IRC.Handshake)message;
            string username = handshake.Username;

            // Generate a user key
            Guid guid = Guid.NewGuid();

            // Add user
            serverChatroom.Users.Add(guid, username);


            // Send the user key to the client
            IRC.Handshake messageShake = new IRC.Handshake(username, guid);

            SocketConnect.Server.Send(e.Client, messageShake);

            // Send user joined message
            IRC.UserJoined messageJoined = new IRC.UserJoined(username);

            server.Broadcast(messageJoined);

        }

        // MESSAGE
        else if (message is IRC.ChatMessage)
        {
            IRC.ChatMessage chatMessage = (IRC.ChatMessage) message;
            serverChatroom.Messages.Add(chatMessage);
            server.Broadcast(message); //?
        }
    };

    server.Start();
});

Thread clientThread = new Thread(() => {
    user1.OnConnect += (s, e) => { };
    user1.OnMessageReceived += (s, e) => {

        SocketConnect.Packet message = e.Message;

        if (false) ;
        else if (message is IRC.ChatMessage)
        {
            IRC.ChatMessage chatMessage = (IRC.ChatMessage)message;
            Console.WriteLine(user1.Username + " recieved: " + chatMessage.Author + ": " + chatMessage.Contents);
        }

        else if (message is IRC.UserJoined)
        {
            IRC.UserJoined userJoined = (IRC.UserJoined)message;
            Console.WriteLine(user1.Username + " recieved: " + userJoined.Username + " Joined!");
        }

        else if (message is IRC.Handshake)
        {
            IRC.Handshake handshake = (IRC.Handshake)message;
            Console.WriteLine(user1.Username + "'s server member id is: " + handshake.Guid);
        }

        else
        {
            Console.WriteLine(user1.Username + " recieved message: " + message.Id);
        }
    };

    user1.Connect();
    user1.ReceiveThread().Start();

    user1.Handshake();

    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE"));
    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE 2222"));
    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE 3"));
    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE 4"));
    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE 5"));
    user1.Send(new SocketConnect.Disconnect());
});

Thread client2Thread = new Thread(() => {
    user2.Connect();
    user2.ReceiveThread().Start();

    user2.Handshake();

    user2.Send(new IRC.ChatMessage(user2.Username, "I am Thomas"));
   // user2.Send(Message.CreateDisconnect());
});


Console.WriteLine("- Start");

//Console.WriteLine("Server Start");
serverThread.Start();

//Console.WriteLine("Client Start");
clientThread.Start();

Thread.Sleep(1000);

//Console.WriteLine("Client2 Start");
client2Thread.Start();

client2Thread.Join();
//Console.WriteLine("Client2 End");

clientThread.Join();
//Console.WriteLine("Client End");

Thread.Sleep(10000);
server.Shutdown();

serverThread.Join();
//Console.WriteLine("Server End");
Console.WriteLine("- End");


//}