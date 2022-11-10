using SocketConnect;
using System.Net;
using System.Threading;

IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
IPAddress ipAddr = ipHost.AddressList[0];


IRC.Chatroom chatroom = new IRC.Chatroom(ipAddr, 11111);

//chatroom.


IRC.User user1 = new IRC.User(ipAddr, 11111, "Ethan");
user1.Connect();

