
using Chess;

Console.WriteLine("Hello, ConcurrentNetworkApplications!");


Board board = new Chess.Board();


board.DebugPrint();
Console.WriteLine();


board.Move(Board.b2, Board.b3, true);


board.DebugPrint();
Console.WriteLine();