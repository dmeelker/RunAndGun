using SdlTest.Components;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Linq;

namespace SdlTest.Entities.Collectables
{
    public abstract class Collectable : Entity
    {
        public readonly PhysicsComponent physicsComponent;
        public readonly Sprite sprite;
        private int yOffset = 0;

        public Collectable(Vector location, Sprite sprite)
        {
            this.sprite = sprite;

            physicsComponent = new PhysicsComponent(this);
            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);
        }

        public override void Render(IntPtr rendererId)
        {
            sprite.Draw(rendererId, (int)Location.X, (int)Location.Y + yOffset);
        }

        public override void Update(uint time, int ticksPassed)
        {
            physicsComponent.Update(ticksPassed, Services.Session.Level);
            yOffset = (int)-Math.Abs(Math.Sin((time / 200)) * 8.0);

            var boundingBox = GetBoundingBox();
            var entityCollisions = Services.EntityManager.FindEntities(boundingBox).ToArray();

            foreach (var entity in entityCollisions)
            {
                if (!(entity is PlayerEntity))
                    continue;

                ((PlayerEntity)entity).Collect(this);
                Dispose();
                return;
            }
        }
    }
}