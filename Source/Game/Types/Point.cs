using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Game.Types
{
    [DebuggerDisplay("{X},{Y}")]
    public struct Point
    {
        public int X;
        public int Y;

        public static readonly Vector Zero = new Vector();

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point
            {
                X = a.X + b.X,
                Y = a.Y + b.Y
            };
        }

        public static Point operator +(Point a, Vector b)
        {
            return new Point
            {
                X = a.X + (int) b.X,
                Y = a.Y + (int) b.Y
            };
        }

        public Point Add(int x, int y)
        {
            return new Point
            {
                X = this.X + x,
                Y = this.Y + y
            };
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point
            {
                X = a.X - b.X,
                Y = a.Y - b.Y
            };
        }

        public static Point operator -(Point a, Vector b)
        {
            return new Point
            {
                X = a.X - (int)b.X,
                Y = a.Y - (int)b.Y
            };
        }

        public static Point operator *(Point a, int scalar)
        {
            return new Point
            {
                X = a.X * scalar,
                Y = a.Y * scalar
            };
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
