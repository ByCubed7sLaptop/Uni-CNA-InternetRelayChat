using CubedChess;

Console.WriteLine("Hello, ConcurrentNetworkApplications!");

Board board = new CubedChess.Board();

board.DebugPrint();
Console.WriteLine();

board.Move(Board.b2, Board.b3);
board.Move(Board.c1, Board.a3);
board.Move(Board.a3, Board.b4);

board.DebugPrint();
Console.WriteLine();

board.DebugPrintNumbers();
Console.WriteLine();


GUI.MainMenu window = new GUI.MainMenu();





//m_Form.ShowDialog();