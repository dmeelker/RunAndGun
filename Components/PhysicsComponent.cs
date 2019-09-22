using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Components
{
    public class PhysicsComponent
    {
        const double tickMultiplier = 0.03;
        public Vector Location;
        public Vector Size;
        public Vector Velocity;
        public Vector Impulse;

        public bool onGround = false;

        public void Update(int ticksPassed, Level level)
        {
            ApplyGravity(ticksPassed);

            var effectiveVelocity = Velocity + Impulse;
            var oldLocation = Location;
            Location = Location + (effectiveVelocity * (ticksPassed * tickMultiplier));

            if (effectiveVelocity.X > 0)
            {
                var startX = (int)(oldLocation.X + Size.X - 1);
                var endX = (int)(Location.X + Size.X);

                for (var x = startX; ; x += Level.BlockSize)
                {
                    x = Math.Min(x, endX);

                    if (!level.IsPixelPassable((int)x, (int)Location.Y + 15))
                    {
                        Location.X = (int)(((x / Level.BlockSize) * Level.BlockSize) - Size.X);
                        Velocity.X = 0;
                        onGround = true;
                        break;
                    }

                    if (x >= endX)
                        break;
                }
            }
            else if (effectiveVelocity.X < 0)
            {
                var startX = (int)oldLocation.X - 1;
                var endX = (int)Location.X - 1;

                for (var x = startX; ; x -= Level.BlockSize)
                {
                    x = Math.Max(x, endX);

                    if (!level.IsPixelPassable((int)x, (int)(Location.Y + (Size.Y / 2))))
                    {
                        Location.X = ((x / Level.BlockSize) + 1) * Level.BlockSize;
                        Velocity.X = 0;
                        onGround = true;
                        break;
                    }

                    if (x <= endX)
                        break;
                }
            }

            if (effectiveVelocity.Y > 0)
            {
                var startY = (int) (oldLocation.Y + Size.Y - 1);
                var endY = (int) (Location.Y + Size.Y);
                onGround = false;

                for (var y = startY;; y += Level.BlockSize)
                {
                    y = Math.Min(y, endY);

                    if (!level.IsPixelPassable((int)(Location.X + (Size.X / 2)), (int)y))
                    {
                        Location.Y = (int) (((y / Level.BlockSize) * Level.BlockSize) - Size.Y);
                        Velocity.Y = 0;
                        onGround = true;
                        break;
                    }

                    if (y >= endY)
                        break;
                }
            }
            else if (effectiveVelocity.Y < 0)
            {
                var startY = (int) oldLocation.Y - 1;
                var endY = (int) Location.Y - 1;
                onGround = false;

                for (var y = startY; ; y -= Level.BlockSize)
                {
                    y = Math.Max(y, endY);

                    if (!level.IsPixelPassable((int)Location.X + 15, (int)y))
                    {
                        Location.Y = ((y / Level.BlockSize) + 1) * Level.BlockSize;
                        Velocity.Y = 0;
                        onGround = true;
                        break;
                    }

                    if (y <= endY)
                        break;
                }
            }

        }

        private void ApplyGravity(int ticksPassed)
        {
            Velocity.Y += 1 * (ticksPassed * tickMultiplier);
            Velocity.Y = Math.Min(Velocity.Y, 15);
        }
    }
}
