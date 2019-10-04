using SdlTest.Components;
using SdlTest.Entities.Collectables;
using SdlTest.Levels;
using SdlTest.Types;
using SdlTest.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SdlTest.Entities
{
    public class Enemy : Entity, IProjectileCollider
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;
        public int Hitpoints = 10;

        public Entity Target;
        public int SenseRange = 400;
        
        public Enemy(Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, Services.SpriteManager["player"], new Pistol() { InfiniteAmmo = true });

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(uint time, int ticksPassed)
        {
            var newTarget = Services.EntityManager.FindEntities(new Rect(Location.X - SenseRange, Location.Y - SenseRange, SenseRange * 2, SenseRange * 2)).Where(entity => entity is PlayerEntity).FirstOrDefault();

            if (newTarget != null && FacingTowardsTarget(newTarget) && CanSeeTarget(newTarget))
            {
                Target = newTarget;
            }
            else if (Target != null && CanSeeTarget(Target))
            {
                Character.AimAt(Target.Location.ToPoint());
                if (Character.Weapon.ReloadNeeded)
                {
                    MoveToCover();
                    Character.Weapon.Reload(time);
                }
                else
                    Character.Fire(time);
            }

            Physics.Update(ticksPassed, Services.Session.Level);
            Physics.Impulse = Vector.Zero;

            Character.Update(time, ticksPassed);
        }

        private void MoveToCover()
        {
        }

        private bool CanSeeTarget(Entity target)
        {
            var vector = target.Location - Location;
            var maxDistance = (int) Math.Min(vector.Length, 500);

            var result = RayCaster.CastRay(Services.Session.Level, Location, vector, maxDistance);
            return !result.Hit;
        }

        private bool FacingTowardsTarget(Entity target)
        {
            if (Character.Direction == Direction.Right && target.Location.X > Location.X)
                return true;
            else if (Character.Direction == Direction.Left && target.Location.X < Location.X)
                return true;
            else
                return false;
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            Character.Render(rendererId, viewOffset);
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location)
        {
            Physics.Impulse += vector.ToUnit() * 10;

            Character.HitByProjectile(projectile, vector, location);
        }
    }
}
