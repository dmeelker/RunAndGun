using SDL2;
using SdlTest.Components;
using SdlTest.Levels;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SdlTest.Entities
{
    public class Projectile : Entity
    {
        private readonly Sprite sprite;
        private readonly Entity source;
        private readonly int maxDistance;
        private readonly Vector creationLocation;
        public PhysicsComponent Physics;
        
        public readonly int Power;

        public Projectile(Entity source, Vector location, Vector velocity, int power, int maxDistance)
        {
            this.sprite = Services.Sprites["projectile"];
            this.source = source;
            this.Power = power;
            this.maxDistance = maxDistance;
            this.creationLocation = location;

            Physics = new PhysicsComponent(this) { 
                applyGravity = false
            };

            Location = location;
            Size = new Vector(8, 8);
            Physics.Velocity = velocity;
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);

            var boundingBox = GetBoundingBox();
            var entityCollisions = Services.EntityManager.FindEntities(boundingBox).ToArray();

            foreach(var entity in entityCollisions)
            {
                if (entity == source || entity == this)
                    continue;

                if(entity is IProjectileCollider)
                {
                    var intersection = boundingBox.Intersect(entity.GetBoundingBox());
                    var hitLocation = new Vector(intersection.X + (intersection.Width / 2), intersection.Y + (intersection.Height / 2)) - entity.Location;

                    ((IProjectileCollider)entity).HitByProjectile(this, Physics.OldVelocity, hitLocation);
                    Dispose();
                    return;
                }
            }

            if (Physics.HorizontalCollision.Collision || Physics.VerticalCollision.Collision)
            {
                Dispose();
                return;
            }

            if ((Location - creationLocation).Length > maxDistance)
            {
                Dispose();
                return;
            }
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, Location.ToPoint() - viewOffset);
        }
    }
}
