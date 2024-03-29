﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Game.Types
{
    public struct Rect
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Vector Location => new Vector(X, Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector point)
        {
            return point.X >= X && point.X < X + Width &&
                point.Y >= Y && point.Y < Y + Height;
        }

        public bool Intersects(Rect other)
        {
            return !(X > other.X + other.Width ||
               X + Width < other.X ||
               Y > other.Y + other.Height ||
               Y + Height < other.Y);
        }

        public Rect Intersect(Rect other)
        {
            var leftX = Math.Max(X, other.X);
            var rightX = Math.Min(X + Width, other.X + other.Width);
            var topY = Math.Max(Y, other.Y);
            var bottomY = Math.Min(Y + Height, other.Y + other.Height);

            return new Rect(leftX, topY, rightX - leftX, bottomY - topY);
        }

        public static Rect CreateFromPoints(Point p1, Point p2)
        {
            var rect = new Rect();
            rect.X = Math.Min(p1.X, p2.X);
            rect.Y = Math.Min(p1.Y, p2.Y);
            rect.Width = Math.Max(p1.X, p2.X) - rect.X;
            rect.Height = Math.Max(p1.Y, p2.Y) - rect.Y;
            return rect;
        }
    }
}
