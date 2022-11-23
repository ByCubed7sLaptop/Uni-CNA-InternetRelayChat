using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubedChess
{

    [Flags]
    public enum Movement
    {
        None = 0,
        Forward = 1,  // Pawn
        DForward = 2,  // Pawn
        L = 4,  // Knight
        RankFile = 8,  // Rook, Queen, King
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
                tiles.Add(Tile.Left + Tile.Up * 2);
                tiles.Add(Tile.Left + Tile.Down * 2);
                tiles.Add(Tile.Right + Tile.Up * 2);
                tiles.Add(Tile.Right + Tile.Down * 2);
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
