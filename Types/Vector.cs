﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SdlTest.Types
{
    [DebuggerDisplay("{X},{Y}")]
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

        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vector ToUnit()
        {
            var length = Length;
            return new Vector {
                X = X / length,
                Y = Y / length
            };
        }

        public double Angle => Math.Atan2(Y, X) * (180.0 / Math.PI);

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
