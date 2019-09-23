using SdlTest.Entities;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Components
{
    public readonly struct LevelCollision
    {
        public readonly bool Collision;
        public readonly int X;
        public readonly int Y;

        public LevelCollision(int x, int y)
        {
            Collision = true;
            X = x;
            Y = y;
        }

        public LevelCollision(bool collision)
        {
            Collision = collision;
            X = 0;
            Y = 0;
        }
    }

    public class PhysicsComponent
    {
        const double tickMultiplier = 0.03;

        private readonly Entity entity;
        public Vector Velocity;
        public Vector Impulse;

        public bool onGround = false;

        public LevelCollision HorizontalCollision;
        public LevelCollision VerticalCollision;

        public PhysicsComponent(Entity entity)
        {
            this.entity = entity;
        }

        public void Update(int ticksPassed, Level level)
        {
            ApplyGravity(ticksPassed);

            var effectiveVelocity = Velocity + Impulse;
            var oldLocation = entity.Location;
            entity.Location = entity.Location + (effectiveVelocity * (ticksPassed * tickMultiplier));
            //Impulse = new Vector();

            HorizontalCollision = new LevelCollision(false);
            VerticalCollision = new LevelCollision(false);

            if (effectiveVelocity.X > 0)
            {
                var startX = (int)(oldLocation.X + entity.Size.X - 1);
                var endX = (int)(entity.Location.X + entity.Size.X);
                var y = (int)(entity.Location.Y + (entity.Size.Y / 2));

                for (var x = startX; ; x += Level.BlockSize)
                {
                    x = Math.Min(x, endX);

                    if (!level.IsPixelPassable(x, y))
                    {
                        var collisionX = (x / Level.BlockSize) * Level.BlockSize;
                        entity.Location.X = (int)(collisionX - entity.Size.X);
                        Velocity.X = 0;
                        onGround = true;

                        HorizontalCollision = new LevelCollision(collisionX, y);
                        break;
                    }

                    if (x >= endX)
                        break;
                }
            }
            else if (effectiveVelocity.X < 0)
            {
                var startX = (int)oldLocation.X - 1;
                var endX = (int)entity.Location.X - 1;
                var y = (int)(entity.Location.Y + (entity.Size.Y / 2));

                for (var x = startX; ; x -= Level.BlockSize)
                {
                    x = Math.Max(x, endX);

                    if (!level.IsPixelPassable(x, y))
                    {
                        var collisionX = ((x / Level.BlockSize) + 1) * Level.BlockSize;
                        entity.Location.X = collisionX;
                        Velocity.X = 0;
                        onGround = true;
                        HorizontalCollision = new LevelCollision(collisionX, y);
                        break;
                    }

                    if (x <= endX)
                        break;
                }
            }

            if (effectiveVelocity.Y > 0)
            {
                var startY = (int) (oldLocation.Y + entity.Size.Y - 1);
                var endY = (int) (entity.Location.Y + entity.Size.Y);
                var x = (int)(entity.Location.X + (entity.Size.X / 2));
                onGround = false;

                for (var y = startY;; y += Level.BlockSize)
                {
                    y = Math.Min(y, endY);

                    if (!level.IsPixelPassable(x, y))
                    {
                        var collisionY = (y / Level.BlockSize) * Level.BlockSize;
                        entity.Location.Y = collisionY - entity.Size.Y;
                        Velocity.Y = 0;
                        onGround = true;
                        VerticalCollision = new LevelCollision(x, collisionY);
                        break;
                    }

                    if (y >= endY)
                        break;
                }
            }
            else if (effectiveVelocity.Y < 0)
            {
                var startY = (int) oldLocation.Y - 1;
                var endY = (int)entity.Location.Y - 1;
                var x = (int)(entity.Location.X + (entity.Size.X / 2));
                onGround = false;

                for (var y = startY; ; y -= Level.BlockSize)
                {
                    y = Math.Max(y, endY);

                    if (!level.IsPixelPassable(x, y))
                    {
                        var collisionY = ((y / Level.BlockSize) + 1) * Level.BlockSize;
                        entity.Location.Y = collisionY;
                        Velocity.Y = 0;
                        onGround = true;
                        VerticalCollision = new LevelCollision(x, collisionY);
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
