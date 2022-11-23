using Chess;

Console.WriteLine("Hello, ConcurrentNetworkApplications!");

Board board = new Chess.Board();

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