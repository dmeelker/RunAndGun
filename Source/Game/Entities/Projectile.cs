using SDL2;
using Game.Entities.Enemies;
using Game.Levels;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Physics;

namespace Game.Entities
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

            Physics = Services.Game.Physics.CreateComponent(this);
            Physics.Velocity = velocity;
            Physics.applyGravity = false;
            Physics.checkType = CollisionCheckType.BlocksProjectiles;

            Location = location;
            Size = new Vector(8, 8);
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Game.Level);

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

            HandleEntityCollisions();

        }

        private void HandleEntityCollisions()
        {
            var boundingBox = GetMovedAreaRect(); // Rect.CreateFromPoints(OldLocation, new P); //GetBoundingBox();
            var entityCollisions = Services.Game.Entities.FindEntities(boundingBox).ToArray();

            foreach (var entity in entityCollisions)
            {
                if (entity == source || entity == this)
                    continue;

                if (entity is IProjectileCollider)
                {
                    if (source is Enemy && entity is Enemy)
                        continue;

                    var intersection = boundingBox.Intersect(entity.GetBoundingBox());
                    var hitLocation = new Vector(intersection.X + (intersection.Width / 2), intersection.Y + (intersection.Height / 2)) - entity.Location;

                    ((IProjectileCollider)entity).HitByProjectile(this, Physics.OldVelocity, hitLocation, source);
                    Dispose();
                    return;
                }
            }
        }

        private Rect GetMovedAreaRect()
        {
            var halfSize = HalfSize;
            var rect = Rect.CreateFromPoints(OldLocation.ToPoint() + halfSize, Location.ToPoint() + halfSize);

            rect.X -= halfSize.X;
            rect.Y -= halfSize.Y;
            rect.Width += Size.X;
            rect.Height += Size.Y;
            return rect;
        }
        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, Location.ToPoint() - viewOffset);
        }

        public override void OnDisposed()
        {
            Services.Game.Physics.DisposeComponent(Physics);
        }
    }
}
