using System.Threading;

SocketConnect.Server server = new SocketConnect.Server();
SocketConnect.Client client = new SocketConnect.Client();


Thread serverThread = new Thread(() => {
    server.Ready();
});

Thread clientThread = new Thread(() => {
    client.Ready();
});


serverThread.Start();
clientThread.Start();

serverThread.Join();
clientThread.Join();



