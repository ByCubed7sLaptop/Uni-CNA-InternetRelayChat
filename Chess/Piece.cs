using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    [Flags]
    public enum PieceType : byte
    {
        None = 0,

        WHITE = 1,
        BLACK = 2,

        PAWN    = 4,
        KNIGHT  = 8,
        BISHOP  = 16,
        ROOK    = 32,
        QUEEN   = 64,
        KING    = 128
    }


    static class PieceTypeMethods
    {
        public static string Ide(this PieceType piece)
        {
            string ide = "";

            if (piece.Is(PieceType.WHITE)) ide += "w";
            if (piece.Is(PieceType.BLACK)) ide += "b";

            if (piece.Is(PieceType.PAWN))   ide += "P";
            if (piece.Is(PieceType.KNIGHT)) ide += "N";
            if (piece.Is(PieceType.BISHOP)) ide += "B";
            if (piece.Is(PieceType.ROOK))   ide += "R";
            if (piece.Is(PieceType.QUEEN))  ide += "Q";
            if (piece.Is(PieceType.KING))   ide += "K";

            return ide;
        }
        public static bool Is(this PieceType piece, PieceType target) => (piece & target) == target;
        public static bool IsWhite(this PieceType piece) => piece.Is(PieceType.WHITE);
        public static bool IsSameSet(this PieceType piece, PieceType target)
        {
            return piece.IsWhite() == target.IsWhite();
        }

        public static Movement GetMovement(this PieceType piece)
        {
            if      (piece.Is(PieceType.PAWN))   return Movement.Forward;
            else if (piece.Is(PieceType.KNIGHT)) return Movement.L;
            else if (piece.Is(PieceType.BISHOP)) return Movement.Diagonal;
            else if (piece.Is(PieceType.ROOK))   return Movement.RankFile;
            else if (piece.Is(PieceType.QUEEN))  return Movement.RankFile | Movement.Diagonal;
            else if (piece.Is(PieceType.KING))   return Movement.RankFile | Movement.Diagonal;
            return Movement.None;
        }
        public static Movement GetCapture(this PieceType piece)
        {
            // Only different for the pawn, same as movement for everything else
            // Does NOT consider en passant capture
            if (piece.Is(PieceType.PAWN)) return Movement.DForward;
            return GetMovement(piece);
        }

        public static int GetValue(this PieceType piece)
        {
            if (piece.Is(PieceType.PAWN)) return 1;
            else if (piece.Is(PieceType.KNIGHT)) return 3;
            else if (piece.Is(PieceType.BISHOP)) return 3;
            else if (piece.Is(PieceType.ROOK))   return 5;
            else if (piece.Is(PieceType.QUEEN))  return 9;
            return 0;
        }
    }


    [Flags]
    public enum Movement
    {
        None     = 0,
        Forward  = 1, // Pawn
        DForward = 2, // Pawn
        L        = 4, // Knight
        RankFile = 8, // Rook, Queen, King
        Diagonal = 16, // Bishop, Queen, King
    }

    static class MovementMethods
    {
        public static List<Tile> Get(this Movement movement, PieceType piece)
        {
            List<Tile> tiles = new List<Tile>();

            if (movement == Movement.None) return tiles;

            // Forward
            if (movement.Is(Movement.Forward))
            {
                tiles.Add(piece.Forward());

                // First move can advance two squares along the same file
                //if (tile.File == (piece.Forward().File * 2 % 8))
                //    tiles.Add(piece.Forward() * 2);
            }

            // DiagonallyForward
            if (movement.Is(Movement.DForward))
            {
                tiles.Add(piece.Forward() + Tile.Right);
                tiles.Add(piece.Forward() + Tile.Left);
            }

            // L
            if (movement.Is(Movement.L))
            {
                tiles.Add(Tile.Up + Tile.Right * 2);
                tiles.Add(Tile.Up + Tile.Left * 2);
                tiles.Add(Tile.Down + Tile.Right * 2);
                tiles.Add(Tile.Down + Tile.Left * 2);
                tiles.Add(Tile.Up * 2 + Tile.Right);
                tiles.Add(Tile.Up * 2 + Tile.Left);
                tiles.Add(Tile.Down * 2 + Tile.Right);
                tiles.Add(Tile.Down * 2 + Tile.Left);
            }

            // RankFile
            if (movement.Is(Movement.RankFile))
            {
                tiles.Add(Tile.Up);
                tiles.Add(Tile.Down);
                tiles.Add(Tile.Left);
                tiles.Add(Tile.Right);
            }

            // Diagonal
            if (movement.Is(Movement.Diagonal))
            {
                tiles.Add(Tile.Up + Tile.Left);
                tiles.Add(Tile.Up + Tile.Right);
                tiles.Add(Tile.Down + Tile.Left);
                tiles.Add(Tile.Down + Tile.Right);
            }

            return tiles;
        }


        // Returns a unit tile pointing it's forward direction
        // This could allows you to have 4 players... for example
        public static Tile Forward(this PieceType piece)
        {
            if (piece.Is(PieceType.WHITE)) return Tile.Up;
            if (piece.Is(PieceType.BLACK)) return Tile.Down;
            return Tile.Zero;
        }

        public static bool IsRepeatable(this Movement movement)
        {
            if (movement.Is(Movement.RankFile)) return true;
            if (movement.Is(Movement.Diagonal)) return true;
            return false;
        }

        public static bool Is(this Movement movement, Movement target)
        {
            return (movement & target) == target;
        }

    }
}
