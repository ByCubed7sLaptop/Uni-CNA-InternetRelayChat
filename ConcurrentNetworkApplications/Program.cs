using CubedChess;
using System;
using System.Windows;

namespace ConcurrentNetworkApplications
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            //GUI.MainMenu window = new GUI.MainMenu();

            //window.Title = "WPF in Console";
            //window.Width = 400;
            //window.Height = 300;
            //window.ShowDialog();



            Console.WriteLine("Hello, ConcurrentNetworkApplications!");

            Board board = new Board();

            board.DebugPrint();
            Console.WriteLine();

            board.Move(Board.b2, Board.b3);
            board.Move(Board.c1, Board.a3);
            board.Move(Board.a3, Board.b4);

            board.DebugPrint();
            Console.WriteLine();

            board.DebugPrintNumbers();
            Console.WriteLine();




            //m_Form.ShowDialog();
        }
    }
}


