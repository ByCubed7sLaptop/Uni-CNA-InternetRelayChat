using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubedChess
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


}
