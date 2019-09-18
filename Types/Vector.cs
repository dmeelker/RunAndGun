using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Types
{
    public struct Vector
    {
        public double X;
        public double Y;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector {
                X = a.X + b.X,
                Y = a.Y + b.Y
            };
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector
            {
                X = a.X - b.X,
                Y = a.Y - b.Y
            };
        }

        public static Vector operator *(Vector a, double scalar)
        {
            return new Vector
            {
                X = a.X * scalar,
                Y = a.Y * scalar
            };
        }
    }
}
