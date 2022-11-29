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
    server.OnPacketReceived += (s, e) => { 
        SocketConnect.Packet message = e.Packet;
        //Console.WriteLine("Chatroom::Chatroom_OnMessageReceived " + message);

        // HANDSHAKE
        if (message is IRC.Handshake handshake)
        {
            // Generate a user key
            Guid guid = Guid.NewGuid();

            // Add user
            serverChatroom.Users.Add(guid, handshake.Username);

            // Send the handshake with the new GUID
            handshake.Guid = Guid.NewGuid();
            SocketConnect.Server.Send(e.Client, handshake);

            // Send user joined message
            IRC.UserJoined messageJoined = new IRC.UserJoined(handshake.Username);

            server.Broadcast(messageJoined);
        }

        // MESSAGE
        else if (message is IRC.ChatMessage chatMessage)
        {
            serverChatroom.Messages.Add(chatMessage);
            server.Broadcast(message); //?
        }
    };

    server.Start();
});

Thread clientThread = new Thread(() => {
    user1.OnConnect += (s, e) => { };
    user1.OnPacketReceived += (s, e) => {

        SocketConnect.Packet message = e.Packet;

        if (message is IRC.ChatMessage chatMessage)
            Console.WriteLine(user1.Name + " recieved: " + chatMessage.Author + ": " + chatMessage.Contents);

        else if (message is IRC.UserJoined userJoined)
            Console.WriteLine(user1.Name + " recieved: " + userJoined.Username + " Joined!");

        else if (message is IRC.Handshake handshake)
            Console.WriteLine(user1.Name + "'s server member id is: " + handshake.Guid);

        else
            ;// Console.WriteLine(user1.Username + " recieved message: " + message.Id);
            
        
    };

    user1.Connect();
    user1.ReceiveThread().Start();

    user1.SendHandshake();

    user1.Send(new IRC.ChatMessage(user1.Name, "TEST MESSAGE"));
    user1.Send(new IRC.ChatMessage(user1.Name, "TEST MESSAGE 2222"));
    user1.Send(new IRC.ChatMessage(user1.Name, "TEST MESSAGE 3"));
    user1.Send(new IRC.ChatMessage(user1.Name, "TEST MESSAGE 4"));
    user1.Send(new IRC.ChatMessage(user1.Name, "TEST MESSAGE 5"));
    user1.Send(new SocketConnect.Disconnect());
});

Thread client2Thread = new Thread(() => {
    user2.Connect();
    user2.ReceiveThread().Start();

    user2.SendHandshake();

    user2.Send(new IRC.ChatMessage(user2.Name, "I am Thomas"));
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