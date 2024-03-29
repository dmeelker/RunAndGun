﻿using Game.Physics;
using Game.Sprites;
using Game.Types;
using System;
using System.Linq;

namespace Game.Entities.Collectables
{
    public abstract class Collectable : Entity
    {
        public readonly PhysicsComponent physicsComponent;
        public readonly Sprite sprite;
        private int yOffset = 0;
        private uint creationTime = 0;

        public Collectable(Vector location, Sprite sprite)
        {
            this.sprite = sprite;
            creationTime = Services.Time;

            physicsComponent = new PhysicsComponent(this);
            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, (Location.ToPoint() - viewOffset).Add(0, yOffset));
        }

        public override void Update(uint time, int ticksPassed)
        {
            physicsComponent.Update(ticksPassed, Services.Game.Level);
            yOffset = (int)-Math.Abs(Math.Sin(((time - creationTime) / 200)) * 8.0);

            var boundingBox = GetBoundingBox();
            var entityCollisions = Services.Game.Entities.FindEntities(boundingBox).ToArray();

            foreach (var entity in entityCollisions)
            {
                if (!(entity is PlayerEntity))
                    continue;

                var player = (PlayerEntity)entity;
                if (!player.CanCollect(this))
                    continue;

                player.Collect(this);
                Dispose();
                return;
            }
        }
    }
}