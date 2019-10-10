using Game.Entities;
using Game.Levels;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Components
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
        public const double tickMultiplier = 0.03;

        private readonly Entity entity;
        public Vector Velocity;
        public Vector Impulse;

        public Vector OldVelocity;

        public bool applyGravity = true;
        public double? drag = null;

        public bool onGround = false;

        public LevelCollision HorizontalCollision;
        public LevelCollision VerticalCollision;

        public PhysicsComponent(Entity entity)
        {
            this.entity = entity;
        }

        public void Update(int ticksPassed, Level level)
        {
            var effectiveVelocity = (Velocity + Impulse) * (ticksPassed * tickMultiplier);
            entity.OldLocation = entity.Location;
            OldVelocity = Velocity;
            HandleMovement(level, effectiveVelocity, entity.OldLocation);

            if(drag.HasValue && onGround && Velocity.X != 0)
            {
                if (Velocity.X > 0)
                    Velocity.X = Math.Max(Velocity.X - (drag.Value * (ticksPassed * tickMultiplier)), 0.0);
                else if (Velocity.X < 0)
                    Velocity.X = Math.Min(Velocity.X + (drag.Value * (ticksPassed * tickMultiplier)), 0.0);
            }

            if (applyGravity && !onGround)
                ApplyGravity(ticksPassed);
        }

        #region Level collisions

        private void HandleMovement(Level level, Vector effectiveVelocity, Vector oldLocation)
        {
            HorizontalCollision = new LevelCollision(false);
            VerticalCollision = new LevelCollision(false);

            HorizontalMovement(level, effectiveVelocity, oldLocation);
            VerticalMovement(level, effectiveVelocity, oldLocation);
        }

        private void HorizontalMovement(Level level, Vector effectiveVelocity, Vector oldLocation)
        {
            entity.Location.X += effectiveVelocity.X;

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
        }


        private LevelCollision CheckLevelCollisionLeft(Vector oldLocation, Level level)
        {
            var startX = (int)oldLocation.X - 1;
            var endX = (int)entity.Location.X - 1;
            var startY = (int) entity.Location.Y;
            var endY = (int) (entity.Location.Y + entity.Size.Y - 1);
            var entities = Services.Game.Entities.FindEntities(new Rect(endX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var x = startX;; x -= Level.BlockSize) {
                x = Math.Max(x, endX);
                
                for (var y = startY;; y += Level.BlockSize) {
                    y = Math.Min(y, endY);
                    var block = level.IsPixelPassable(x, y);
                    if (block == BlockType.Block || block == BlockType.StairRight)
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
            var entities = Services.Game.Entities.FindEntities(new Rect(startX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var x = startX;; x += Level.BlockSize) {
                x = Math.Min(x, endX);

                for (var y = startY;; y += Level.BlockSize) {
                    y = Math.Min(y, endY);
                    var block = level.IsPixelPassable(x, y);
                    if (block == BlockType.Block || block == BlockType.StairLeft)
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

        private void VerticalMovement(Level level, Vector effectiveVelocity, Vector oldLocation)
        {
            entity.Location.Y += effectiveVelocity.Y;

            if (effectiveVelocity.Y >= 0)
            {
                onGround = false;
                //VerticalCollision = CheckRampCollisionsBottom(level, oldLocation);
                //if (VerticalCollision.Collision)
                //{
                //    entity.Location.Y = VerticalCollision.Y - entity.Size.Y;
                //    Velocity.Y = 0;
                //    onGround = true;
                //}
                //else
                //{
                    VerticalCollision = CheckLevelCollisionBottom(level, oldLocation);

                    if (VerticalCollision.Collision)
                    {
                        entity.Location.Y = VerticalCollision.Y - entity.Size.Y;
                        Velocity.Y = 0;
                        onGround = true;
                    }
                //}
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

                    if (level.IsPixelPassable(x, y) != BlockType.Open)
                        return new LevelCollision(x, ((y / Level.BlockSize) + 1) * Level.BlockSize);

                    if (x >= endX)
                        break;
                }

                if (y <= endY)
                    break;
            }

            return new LevelCollision(false);
        }

        private LevelCollision CheckRampCollisionsBottom(Level level, Vector oldLocation)
        {
            var startY = (int)(oldLocation.Y + entity.Size.Y - 1);
            var endY = (int)(entity.Location.Y + entity.Size.Y);
            var x = (int)(entity.Location.X + (entity.Size.X / 2));

            for (var y = startY; ; y += Level.BlockSize)
            {
                y = Math.Min(y, endY);

                var block = level.IsPixelPassable(x, y);
                var collisionY = (y / Level.BlockSize) * Level.BlockSize;

                if (block == BlockType.StairRight)
                {
                    var yOffset = Level.BlockSize - (x % Level.BlockSize);

                    return new LevelCollision(x, collisionY + yOffset - 2);
                }
                else if (block == BlockType.StairLeft)
                {
                    var yOffset = x % Level.BlockSize;
                    return new LevelCollision(x, collisionY + yOffset - 2);
                }

                if (y >= endY)
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
            var entities = Services.Game.Entities.FindEntities(new Rect(startX, startY, endX - startX, endY - startY)).OfType<IPhysicsCollider>().Cast<Entity>().ToArray();

            for (var y = startY; ; y += Level.BlockSize)
            {
                y = Math.Min(y, endY);

                for (var x = startX; ; x += Level.BlockSize)
                {
                    x = Math.Min(x, endX);
                    var block = level.IsPixelPassable(x, y);
                    var collisionY = (y / Level.BlockSize) * Level.BlockSize;

                    if (block == BlockType.Block)
                        return new LevelCollision(x, collisionY);

                    foreach (var entity in entities)
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
