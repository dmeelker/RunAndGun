using SdlTest.Entities;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
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

            CheckLevelCollisions(level, effectiveVelocity, oldLocation);
        }

        #region Level collisions

        private void CheckLevelCollisions(Level level, Vector effectiveVelocity, Vector oldLocation)
        {
            HorizontalCollision = new LevelCollision(false);
            VerticalCollision = new LevelCollision(false);

            if (effectiveVelocity.X < 0)
            {
                HorizontalCollision = CheckLevelCollisionLeft(oldLocation, level);
                if (HorizontalCollision.Collision)
                {
                    entity.Location.X = HorizontalCollision.X;
                    Velocity.X = 0;
                }
            }
            else if (effectiveVelocity.X > 0)
            {
                HorizontalCollision = CheckLevelCollisionRight(oldLocation, level);
                if (HorizontalCollision.Collision)
                {
                    entity.Location.X = (int)(HorizontalCollision.X - entity.Size.X);
                    Velocity.X = 0;
                }
            }

            if (effectiveVelocity.Y > 0)
            {
                onGround = false;
                VerticalCollision = CheckLevelCollisionBottom(level, oldLocation);

                if (VerticalCollision.Collision)
                {
                    entity.Location.Y = VerticalCollision.Y - entity.Size.Y;
                    Velocity.Y = 0;
                    onGround = true;
                }
            }
            else if (effectiveVelocity.Y < 0)
            {
                onGround = false;
                VerticalCollision = CheckLevelCollisionTop(level, oldLocation);
                if (VerticalCollision.Collision)
                {
                    entity.Location.Y = VerticalCollision.Y;
                    Velocity.Y = 0;
                }
            }
        }

        private LevelCollision CheckLevelCollisionLeft(Vector oldLocation, Level level)
        {
            var startX = (int)oldLocation.X - 1;
            var endX = (int)entity.Location.X - 1;
            var startY = (int) entity.Location.Y;
            var endY = (int) (entity.Location.Y + entity.Size.Y - 1);
            var entities = Services.EntityManager.FindEntities(new Rect(endX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var x = startX;; x -= Level.BlockSize) {
                x = Math.Max(x, endX);

                for (var y = startY;; y += Level.BlockSize) {
                    y = Math.Min(y, endY);

                    if (!level.IsPixelPassable(x, y))
                        return new LevelCollision(((x / Level.BlockSize) + 1) * Level.BlockSize, y);

                    foreach (var entity in entities)
                    {
                        if (entity.GetBoundingBox().Contains(new Vector(x, y)))
                            return new LevelCollision((int)(entity.Location.X + entity.Size.X), y);
                    }

                    if (y >= endY)
                        break;
                }

                if (x <= endX)
                    break;
            }

            return new LevelCollision(false);
        }

        private LevelCollision CheckLevelCollisionRight(Vector oldLocation, Level level)
        {
            var startX = (int)(oldLocation.X + entity.Size.X - 1);
            var endX = (int)(entity.Location.X + entity.Size.X);
            var startY = (int) entity.Location.Y;
            var endY = (int)(entity.Location.Y + entity.Size.Y - 1);
            var entities = Services.EntityManager.FindEntities(new Rect(startX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var x = startX;; x += Level.BlockSize) {
                x = Math.Min(x, endX);

                for (var y = startY;; y += Level.BlockSize) {
                    y = Math.Min(y, endY);
                    if (!level.IsPixelPassable(x, y))
                        return new LevelCollision((x / Level.BlockSize) * Level.BlockSize, y);

                    foreach (var entity in entities)
                    {
                        if (entity.GetBoundingBox().Contains(new Vector(x, y)))
                            return new LevelCollision((int)entity.Location.X, y);
                    }

                    if (y >= endY)
                        break;
                }

                if (x >= endX)
                    break;
            }

            return new LevelCollision(false);
        }
        private LevelCollision CheckLevelCollisionTop(Level level, Vector oldLocation)
        {
            var startY = (int)oldLocation.Y - 1;
            var endY = (int)entity.Location.Y - 1;
            var startX = (int) entity.Location.X;
            var endX = (int)(entity.Location.X + entity.Size.X - 1);

            for (var y = startY; ; y -= Level.BlockSize)
            {
                y = Math.Max(y, endY);

                for (var x = startX; ; x += Level.BlockSize)
                {
                    x = Math.Min(x, endX);

                    if (!level.IsPixelPassable(x, y))
                        return new LevelCollision(x, ((y / Level.BlockSize) + 1) * Level.BlockSize);

                    if (x >= endX)
                        break;
                }

                if (y <= endY)
                    break;
            }

            return new LevelCollision(false);
        }

        private LevelCollision CheckLevelCollisionBottom(Level level, Vector oldLocation)
        {
            var startY = (int)(oldLocation.Y + entity.Size.Y - 1);
            var endY = (int)(entity.Location.Y + entity.Size.Y);
            var startX = (int)entity.Location.X;
            var endX = (int)(entity.Location.X + entity.Size.X - 1);
            var entities = Services.EntityManager.FindEntities(new Rect(startX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var y = startY; ; y += Level.BlockSize)
            {
                y = Math.Min(y, endY);

                for (var x = startX; ; x += Level.BlockSize)
                {
                    x = Math.Min(x, endX);
                    if (!level.IsPixelPassable(x, y))
                        return new LevelCollision(x, (y / Level.BlockSize) * Level.BlockSize);

                    foreach(var entity in entities)
                    {
                        if(entity.GetBoundingBox().Contains(new Vector(x, y)))
                            return new LevelCollision(x, (int) entity.Location.Y);
                    }

                    if (x >= endX)
                        break;
                }

                if (y >= endY)
                    break;
            }

            return new LevelCollision(false);
        }

        #endregion

        private void ApplyGravity(int ticksPassed)
        {
            Velocity.Y += 1 * (ticksPassed * tickMultiplier);
            Velocity.Y = Math.Min(Velocity.Y, 15);
        }
    }
}
