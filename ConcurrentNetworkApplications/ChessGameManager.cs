using System.Drawing;

namespace ConcurrentNetworkApplications
{
    public class ChessGameCollectionManager
    {
        // In theory, you could play multiple games at once
        public Dictionary<(Guid, Guid), Chess.ChessGame> chessGames;

        public ChessGameCollectionManager()
        {
            chessGames = new Dictionary<(Guid, Guid), Chess.ChessGame>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chessGameUpdate"></param>
        /// <returns>The chess game that was updated.</returns>
        public Chess.ChessGame? HandlePacket(ChessGameUpdate chessGameUpdate)
        {
            if (chessGameUpdate is ChessGameCreate create)
            {
                Chess.ChessGame newChessGame = new Chess.ChessGame(100, true);

                // Add new game to game list
                chessGames[(create.Player1, create.Player2)] = newChessGame;

                // Hook into events... ect

                return newChessGame;
            }

            else if (chessGameUpdate is ChessGameMove move)
            {
                // Update the board with the appropreite state
                // TODO: Double check this isnt just being copied when your not on 0hrs sleep
                Chess.ChessGame game = chessGames[(move.Player1, move.Player2)]; 
                game.SelectPeice(move.SelectedPiece);
                game.MakeMove(move.MovedToTile);
                return game;
            }

            else if (chessGameUpdate is ChessGameEnd end)
            {
                // Get who won

                // ...

                // Remove the game
                chessGames.Remove((end.Player1, end.Player2));
            }

            return null;
        }
    }

    public class ChessGameMove : ChessGameUpdate {
        public Point SelectedPiece { get; set; }
        public Point MovedToTile { get; set; }
    }
    public class ChessGameEnd : ChessGameUpdate { 
        // Why the game ended
        public Chess.ChessGame.EndGames EndGames { get; set; }
    }

    public class ChessGameCreate : ChessGameUpdate { }

    public class ChessGameUpdate : SocketConnect.Packet
    {
        public Guid Player1 { get; set; }
        public Guid Player2 { get; set; }

        // Was the action performed by player 1
        public bool IsPlayer1 { get; set; } = true;
    }

}