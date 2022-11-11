using SocketConnect;
using System.Net;
using System.Threading;

IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddr = ipHost.AddressList[0];
int port = 11111;

IRC.Chatroom serverChatroom = new IRC.Chatroom();
SocketConnect.Server server = new SocketConnect.Server(ipAddr, port);

//chatroom.

IRC.User user1 = new IRC.User(ipAddr, port, "Ethan");
IRC.User user2 = new IRC.User(ipAddr, port, "Thomas");


Thread serverThread = new Thread(() => {
    // Hook into events
    server.OnConnect += (s, e) => { serverChatroom._OnConnect(server, e); };
    server.OnMessageReceived += (s, e) => { serverChatroom._OnMessageReceived(server, e); };

    server.Start();
});

Thread clientThread = new Thread(() => {
    user1.Connect();
    user1.ReceiveThread().Start();

    user1.Handshake();

    user1.Send(new IRC.ChatMessage(user1.Username, "TEST MESSAGE"));
    user1.Send(new IRC.ChatMessage(user1.Username, "AAAAAAAAAAAA"));
    user1.Send(new IRC.ChatMessage(user1.Username, "BBBBBBBBBB"));
    user1.Send(new IRC.ChatMessage(user1.Username, "AAAAAAAAAAAA"));
    user1.Send(new IRC.ChatMessage(user1.Username, "AAAAAAAAAAAA"));
    //user1.Send(Message.CreateDisconnect());
});

Thread client2Thread = new Thread(() => {
    user2.Connect();
    user2.ReceiveThread().Start();

    user2.Handshake();

    user2.Send(new IRC.ChatMessage(user2.Username, "TEST MESSAGE 2"));
    user2.Send(Message.CreateDisconnect());
});


Console.WriteLine("- Start");

//Console.WriteLine("Server Start");
serverThread.Start();

//Console.WriteLine("Client2 Start");
client2Thread.Start();

//Console.WriteLine("Client Start");
clientThread.Start();

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