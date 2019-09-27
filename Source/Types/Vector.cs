using System;
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

        public static readonly Vector Zero = new Vector();

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector FromAngleInDegrees(double angle)
        {
            var radians = Angles.ToRadians(angle);
            return new Vector(Math.Cos(radians), Math.Sin(radians));
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

        public double AngleInDegrees => Angles.ToDegrees(Math.Atan2(Y, X));

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
