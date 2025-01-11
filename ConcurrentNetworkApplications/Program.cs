using System.Net;

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

            //ServerApplication serverApp = new ServerApplication(ipAddr, port);
            //serverApp.Run();

            ClientApplication clientApp = new ClientApplication(ipAddr, port, "Braien 2");
            clientApp.Run();
            clientApp.ShowDialog();

            //new Thread(() => { 
            //    ClientApplication clientApp2 = new ClientApplication(ipAddr, port, "AAAAAAAAA");
            //    Thread.Sleep(1000);
            //    clientApp2.Run(); 
            //}).Start();



            //ClientApplication clientApp2 = new ClientApplication(ipHost, ipAddr, port);
            //clientApp2.user.Username = "Thomas";
            //clientApp2.Run();


        }



    }
}


