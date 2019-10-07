using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Editor.Types
{
    public static class PointExtensions
    {
        public static Point Add(this Point p1, Point p2)
        {
            return new Point
            {
                X = p1.X + p2.X,
                Y = p1.Y + p2.Y
            };
        }

        public static Point Subtract(this Point p1, Point p2)
        {
            return new Point
            {
                X = p1.X - p2.X,
                Y = p1.Y - p2.Y
            };
        }

        public static Point Divide(this Point p1, int amount)
        {
            return new Point
            {
                X = p1.X / amount,
                Y = p1.Y / amount
            };
        }
    }
}
