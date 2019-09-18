using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Types
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
    }
}
