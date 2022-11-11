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

        public static Tile operator +(Tile v1, Tile v2)
        {
            return new Tile(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Tile operator -(Tile v1, Tile v2)
        {
            return new Tile(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Tile operator *(Tile v1, int m)
        {
            return new Tile(v1.X * m, v1.Y * m);
        }

        public static Tile operator /(Tile v1, int m)
        {
            return new Tile(v1.X / m, v1.Y / m);
        }

        public static int Distance(Tile v1, Tile v2)
        {
            return (int)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
    }
}
