using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public static class Angles
    {
        private const double radiansPerDegree = 180.0 / Math.PI;
        private const double degreesPerRadian = Math.PI / 180.0;
        public static double ToDegrees(double radians)
        {
            return radians * radiansPerDegree;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * degreesPerRadian;
        }
    }
}
