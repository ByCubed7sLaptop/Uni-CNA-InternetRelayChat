using SocketConnect;
using System.Net;
using System.Threading;


//for (int i = 0; i < 100; i++) {
IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddr = ipHost.AddressList[0];
//IPEndPoint endPoint = new IPEndPoint(ipAddr, 11111);

SocketConnect.Server server = new SocketConnect.Server(ipAddr);
Thread serverThread = new Thread(() => {
    server.Start();
});

SocketConnect.Client client = new SocketConnect.Client(ipAddr);
Thread clientThread = new Thread(() => {
    client.Connect();

    client.Send(new Message().Titled("Message1#A"));
    client.Send(new Message().Titled("Message1#B").Add("AAAAA"));
    client.Send(Message.CreateDisconnect());
});

SocketConnect.Client client2 = new SocketConnect.Client(ipAddr);
Thread client2Thread = new Thread(() => {
    client2.Connect();

    client2.Send(new Message().Titled("Message2#A").Add("ByCubed7").Add("AAAAAAAAAAAAAAAAAA"));
    client2.Send(new Message().Titled("Message2#B"));
    client2.Send(new Message().Titled("Message2#C"));
    //client.Send(Message.CreateShutdown());
    client2.Send(Message.CreateDisconnect());

});


Console.WriteLine("Server Start");
serverThread.Start();

Console.WriteLine("Client Start");
clientThread.Start();

Console.WriteLine("Client2 Start");
client2Thread.Start();

client2Thread.Join();
Console.WriteLine("Client2 End");

clientThread.Join();
Console.WriteLine("Client End");

server.Shutdown();

serverThread.Join();
Console.WriteLine("Server End");


//}