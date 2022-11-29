using SocketConnect;
using System.Net;
using System.Threading;

//for (int i = 0; i < 100; i++) {
IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddr = ipHost.AddressList[0];
int port = 11111;
//IPEndPoint endPoint = new IPEndPoint(ipAddr, 11111);

SocketConnect.Server server = new SocketConnect.Server(ipAddr, port);
Thread serverThread = new Thread(() => {
    server.Start();
});

SocketConnect.Client client = new SocketConnect.Client(ipAddr, port);
Thread clientThread = new Thread(() => {
    client.Connect();
    client.ReceiveThread().Start();

    //client.Send(new Packet());
    //client.Send(new Message().Titled("Message1#B").Add("AAAAA"));
    client.Send(new Disconnect());
});

SocketConnect.Client client2 = new SocketConnect.Client(ipAddr, port);
Thread client2Thread = new Thread(() => {
    client2.Connect();
    client2.ReceiveThread().Start();

    //client2.Send(new Message().Titled("Message2#A").Add("ByCubed7").Add("AAAAAAAAAAAAAAAAAA"));
    //client2.Send(new Message().Titled("Message2#B"));
    //client2.Send(new Message().Titled("Message2#C"));
    //client.Send(new Shutdown());
    client2.Send(new Disconnect());

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

//Thread.Sleep(1000); 
server.Shutdown();

serverThread.Join();
Console.WriteLine("Server End");


//}