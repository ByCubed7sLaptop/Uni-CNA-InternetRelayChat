﻿using SocketConnect;
using System.Threading;

SocketConnect.Server server = new SocketConnect.Server();
SocketConnect.Client client = new SocketConnect.Client();
SocketConnect.Client client2 = new SocketConnect.Client();


Thread serverThread = new Thread(() => {
    server.Start();
});

Thread clientThread = new Thread(() => {
    client.Handshake();

    Thread.Sleep(100);
    client.Send(new Message().Titled("Message1#A"));
    Thread.Sleep(100);
    client.Send(new Message().Titled("Message1#B"));
    Thread.Sleep(100);
    client.Send(Message.CreateDisconnect());
});

Thread client2Thread = new Thread(() => {
    client2.Handshake();

    Thread.Sleep(100);
    client.Send(new Message().Titled("Message2#A"));
    Thread.Sleep(100);
    client.Send(new Message().Titled("Message2#B"));
    Thread.Sleep(100);
    client.Send(new Message().Titled("Message2#C"));
    Thread.Sleep(100);
    //client.Send(Message.CreateShutdown());
    client.Send(Message.CreateDisconnect());

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

Thread.Sleep(1000);
server.Shutdown();

serverThread.Join();
Console.WriteLine("Server End");


