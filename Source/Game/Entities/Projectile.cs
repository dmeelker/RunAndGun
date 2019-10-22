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
using Game.Particle;

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
        private ConstantEmitter particleEmitter;

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

            CreateTrailEmitter();

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
            UpdateParticleEmitterLocation();
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

        private void CreateTrailEmitter()
        {
            var particleSprite = Services.Sprites["round-particle"];
            particleEmitter = new ConstantEmitter(Services.Time)
            {
                ParticleInterval = 1,
                ParticleFactory = (time) => new Particle.Particle(time)
                {
                    MaxAge = 80,
                    Sprite = particleSprite,
                    Velocity = new Vector(0, 0),
                    ScaleFunction = EasingFunctions.AnimateScalar(.2, .05, EasingFunctions.EaseInQuad),
                    ColorFunction = EasingFunctions.AnimateColor(new Color(255, 241, 181, 255), new Color(135, 51, 0, 255), EasingFunctions.EaseOutCubic)
                }
            };
            UpdateParticleEmitterLocation();

            Services.Game.Particles.AddEmitter(particleEmitter);
        }

        private void UpdateParticleEmitterLocation()
        {
            particleEmitter.Location = CenterLocation;
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
            particleEmitter.Dispose();
        }
    }
}
