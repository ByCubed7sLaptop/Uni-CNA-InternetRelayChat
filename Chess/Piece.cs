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

            if ((piece & PieceType.WHITE) == PieceType.WHITE) ide += "w";
            if ((piece & PieceType.BLACK) == PieceType.BLACK) ide += "b";

            if ((piece & PieceType.PAWN)    == PieceType.PAWN)  ide += "P";
            if ((piece & PieceType.KNIGHT)  == PieceType.KNIGHT) ide += "N";
            if ((piece & PieceType.BISHOP)  == PieceType.BISHOP) ide += "B";
            if ((piece & PieceType.ROOK)    == PieceType.ROOK)  ide += "R";
            if ((piece & PieceType.QUEEN)   == PieceType.QUEEN) ide += "Q";
            if ((piece & PieceType.KING)    == PieceType.KING)  ide += "K";

            return ide;
        }
        public static Movement GetMovement(this PieceType piece)
        {
            if      ((piece & PieceType.PAWN)   == PieceType.PAWN)   return Movement.Forward;
            else if ((piece & PieceType.KNIGHT) == PieceType.KNIGHT) return Movement.L;
            else if ((piece & PieceType.BISHOP) == PieceType.BISHOP) return Movement.Diagonal;
            else if ((piece & PieceType.ROOK)   == PieceType.ROOK)   return Movement.RankFile;
            else if ((piece & PieceType.QUEEN)  == PieceType.QUEEN)  return Movement.RankFile | Movement.Diagonal;
            else if ((piece & PieceType.KING)   == PieceType.KING)   return Movement.RankFile | Movement.Diagonal;
            return Movement.None;
        }
        public static Movement GetCapture(this PieceType piece)
        {
            // Only different for the pawn, same as movement for everything else
            // Does NOT consider en passant capture
            if      ((piece & PieceType.PAWN)   == PieceType.PAWN)   return Movement.DiagonallyForward;
            return GetMovement(piece);
        }

        public static int GetValue(this PieceType piece)
        {
            if      ((piece & PieceType.PAWN)   == PieceType.PAWN)   return 1;
            else if ((piece & PieceType.KNIGHT) == PieceType.KNIGHT) return 3;
            else if ((piece & PieceType.BISHOP) == PieceType.BISHOP) return 3;
            else if ((piece & PieceType.ROOK)   == PieceType.ROOK)   return 5;
            else if ((piece & PieceType.QUEEN)  == PieceType.QUEEN)  return 9;
            return 0;
        }
    }


    [Flags]
    public enum Movement
    {
        None,
        Forward, // Pawn
        DiagonallyForward, // Pawn
        L, // Knight
        RankFile, // Rook, Queen, King
        Diagonal, // Bishop, Queen, King
    }

    static class MovementMethods
    {
        public static List<Tile> Get(this Movement movement, Tile tile, PieceType piece)
        {
            int range = 8;
            List<Tile> tiles = new List<Tile>();

            if (movement == Movement.None) return tiles;

            // Forward
            if ((movement & Movement.Forward) == Movement.Forward)
            {
                tiles.Add(tile + piece.Forward());

                // First move can advance two squares along the same file
                if (tile.File == (piece.Forward().File * 2 % range))
                    tiles.Add(tile + piece.Forward() * 2);
            }

            // DiagonallyForward
            if ((movement & Movement.DiagonallyForward) == Movement.DiagonallyForward)
            {
                tiles.Add(tile + piece.Forward() + Tile.Right);
                tiles.Add(tile + piece.Forward() + Tile.Left);
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
            if ((movement & Movement.RankFile) == Movement.RankFile)
            {
                tiles.Add(tile + Tile.Up);
                tiles.Add(tile + Tile.Down);
                tiles.Add(tile + Tile.Left);
                tiles.Add(tile + Tile.Right);
            }

            // Diagonal
            if ((movement & Movement.Diagonal) == Movement.Diagonal)
            {
                tiles.Add(tile + Tile.Up + Tile.Left);
                tiles.Add(tile + Tile.Up + Tile.Right);
                tiles.Add(tile + Tile.Down + Tile.Left);
                tiles.Add(tile + Tile.Down + Tile.Right);
            }

            return tiles;
        }


        // Returns a unit tile pointing it's forward direction
        // This could allows you to have 4 players... for example
        public static Tile Forward(this PieceType piece)
        {
            if ((piece & PieceType.WHITE) == PieceType.WHITE) return Tile.Up;
            if ((piece & PieceType.BLACK) == PieceType.BLACK) return Tile.Down;
            return Tile.Zero;
        }

        public static bool IsRepeatable(this Movement movement)
        {
            if ((movement & Movement.RankFile) == Movement.RankFile) return true;
            if ((movement & Movement.Diagonal) == Movement.Diagonal) return true;
            return false;
        }
    }
}
