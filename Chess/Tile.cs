using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public struct Tile
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Rank => X;
        public int File => Y;


        public static readonly Tile Up    = new( 0,  1);
        public static readonly Tile Down  = new( 0, -1);
        public static readonly Tile Left  = new(-1,  0);
        public static readonly Tile Right = new( 1,  0);
        public static readonly Tile Zero  = new( 0,  0);
        public static readonly Tile One   = new( 1,  1);

        public Tile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool InRange(int range)
        {
            return X >= 0 & Y >= 0 & X < range & Y < range;
        }

        public static Tile operator +(Tile tile1, Tile tile2)
        {
            return new Tile(tile1.X + tile2.X, tile1.Y + tile2.Y);
        }

        public static Tile operator -(Tile tile1, Tile tile2)
        {
            return new Tile(tile1.X - tile2.X, tile1.Y - tile2.Y);
        }

        public static Tile operator *(Tile tile1, int m)
        {
            return new Tile(tile1.X * m, tile1.Y * m);
        }

        public static Tile operator /(Tile tile1, int m)
        {
            return new Tile(tile1.X / m, tile1.Y / m);
        }

        public static double Distance(Tile tile1, Tile tile2)
        {
            return Math.Sqrt(DistanceSquared(tile1, tile2));
        }

        public static double DistanceSquared(Tile tile1, Tile tile2)
        {
            return Math.Pow(tile1.X - tile2.X, 2) + Math.Pow(tile1.Y - tile2.Y, 2);
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public int LengthSquared()
        {
            return X * X + Y * Y;
        }
    }
}
