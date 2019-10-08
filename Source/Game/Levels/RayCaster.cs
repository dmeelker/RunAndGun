using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Levels
{
    public readonly struct RayResult
    {
        public readonly bool Hit;
        public readonly Vector HitLocation;

        public RayResult(bool hit, Vector hitLocation)
        {
            Hit = hit;
            HitLocation = hitLocation;
        }
    }

    public static class RayCaster
    {
        public static RayResult CastRay(Level level, Vector location, Vector vector, int maxDistance = 1000)
        {
            vector = vector.ToUnit() * 10;

            var currentLocation = location;
            var step = 0;
            while (step < maxDistance / 10)
            {
                step++;
                currentLocation += vector;

                if (Services.Game.Level.IsPixelPassable((int)currentLocation.X, (int)currentLocation.Y) == BlockType.Block)
                    return new RayResult(true, currentLocation);
            }

            return new RayResult(false, Vector.Zero);
        }
    }
}
