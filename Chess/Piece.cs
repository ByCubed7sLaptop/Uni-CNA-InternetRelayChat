using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct Piece
    {
        public PieceType PieceType { get; set; }
        public Set Set { get; set; }

        public Piece(PieceType pieceType, Set set)
        {
            PieceType = pieceType;
            Set = set;
        }
    }

    public enum PieceType
	{
		PAWN,
		KNIGHT,
		BISHOP,
		ROOK,
		QUEEN,
		KING
	}

    [Flags]
    public enum Movement
    {
        None,
        Forward, // Pawn
        DiagonallyForward, // Pawn
        L, // Knight
        RankFile, // Rook, Queen
        Diagonal, // Bishop, Queen
        Surround // King
    }


    static class PieceTypeMethods
    {
        public static string Ide(this PieceType piece)
        {
            switch (piece)
            {
                case PieceType.PAWN: return "P";
                case PieceType.KNIGHT: return "N";
                case PieceType.BISHOP: return "B";
                case PieceType.ROOK: return "R";
                case PieceType.QUEEN: return "Q";
                case PieceType.KING: return "K";
                default: return "?";
            }
        }
        public static Movement GetMovement(this PieceType piece)
        {
            switch (piece)
            {
                case PieceType.PAWN: return Movement.Forward;
                case PieceType.KNIGHT: return Movement.L;
                case PieceType.BISHOP: return Movement.Diagonal;
                case PieceType.ROOK: return Movement.RankFile;
                case PieceType.QUEEN: return Movement.RankFile | Movement.Diagonal;
                case PieceType.KING: return Movement.Surround;
                default: return Movement.None;
            }
        }
        public static Movement GetCapture(this PieceType piece)
        {
            // Only different for the pawn, same as movement for everything else
            // Does NOT consider en passant capture
            if (piece == PieceType.PAWN) return Movement.DiagonallyForward;
            return GetMovement(piece);
        }

        public static int GetValue(this PieceType piece)
        {
            switch (piece)
            {
                case PieceType.PAWN: return 1;
                case PieceType.KNIGHT: return 3;
                case PieceType.BISHOP: return 3;
                case PieceType.ROOK: return 5;
                case PieceType.QUEEN: return 9;
                case PieceType.KING: return 0;
                default: return 0;
            }
        }
    }


    static class MovementMethods
    {

        public static List<Tile> Get(this Movement movement, Tile tile, Set set, Tile range)
        {
            List<Tile> tiles = new List<Tile>();

            if (movement == Movement.None) return tiles;

            // Forward
            if ((movement & Movement.Forward) == Movement.Forward)
            {
                tiles.Add(tile + set.Forward());

                // First move can advance two squares along the same file
                if (tile.File == (set.Forward().File * 2 % range.File))
                    tiles.Add(tile + set.Forward() * 2);
            }

            // DiagonallyForward
            if ((movement & Movement.DiagonallyForward) == Movement.DiagonallyForward)
            {
                tiles.Add(tile + set.Forward() + Tile.Right);
                tiles.Add(tile + set.Forward() + Tile.Left);
            }

            // L
            if ((movement & Movement.L) == Movement.L)
            {
                tiles.Add(tile + Tile.Up + Tile.Right * 2);
                tiles.Add(tile + Tile.Up + Tile.Left * 2);
                tiles.Add(tile + Tile.Down + Tile.Right * 2);
                tiles.Add(tile + Tile.Down + Tile.Left * 2);
                tiles.Add(tile + Tile.Up * 2 + Tile.Right);
                tiles.Add(tile + Tile.Up * 2 + Tile.Left);
                tiles.Add(tile + Tile.Down * 2 + Tile.Right);
                tiles.Add(tile + Tile.Down * 2 + Tile.Left);
            }

            // RankFile
            if ((movement & Movement.L) == Movement.L)
            {
                for (int x = 0; x < range.X; x++) tiles.Add(new Tile(x, tile.Y));
                for (int y = 0; y < range.Y; y++) tiles.Add(new Tile(tile.X, y));
            }

            // Diagonal
            if ((movement & Movement.Diagonal) == Movement.Diagonal)
            {
                int dia = Math.Min(range.X, range.Y);
                for (int i = 0; i < dia; i++) tiles.Add(new Tile(i, i + tile.Y - tile.X));
                for (int i = 0; i < dia; i++) tiles.Add(new Tile(i + tile.X - tile.Y, i));
            }

            // Surround
            if ((movement & Movement.Surround) == Movement.Surround)
            {
                tiles.Add(tile + Tile.Up);
                tiles.Add(tile + Tile.Down);
                tiles.Add(tile + Tile.Left);
                tiles.Add(tile + Tile.Right);
                tiles.Add(tile + Tile.Up + Tile.Left);
                tiles.Add(tile + Tile.Up + Tile.Right);
                tiles.Add(tile + Tile.Down + Tile.Left);
                tiles.Add(tile + Tile.Down + Tile.Right);
            }

            return tiles;
        }
    }
}
