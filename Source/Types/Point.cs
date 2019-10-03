﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SdlTest.Types
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

        public static Point operator -(Point a, Point b)
        {
            return new Point
            {
                X = a.X - b.X,
                Y = a.Y - b.Y
            };
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
