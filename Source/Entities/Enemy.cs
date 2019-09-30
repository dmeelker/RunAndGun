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
            Target = Services.EntityManager.FindEntities(new Rect(Location.X - SenseRange, Location.Y - SenseRange, SenseRange * 2, SenseRange * 2)).Where(entity => entity is PlayerEntity).FirstOrDefault();

            if(Target != null && FacingTowardsTarget(Target) && CanSeeTarget(Target))
            {
                Character.AimAt((int)Target.Location.X, (int)Target.Location.Y);
                if (Character.Weapon.ReloadNeeded)
                {
                    MoveToCover();
                    Character.Weapon.Reload(time);
                }
                else
                    Character.Fire(time);
            }

            //if (Character.Direction == Direction.Right)
            //{
            //    Physics.Impulse.X += 5;
            //} 
            //else if (Character.Direction == Direction.Left)
            //{
            //    Physics.Impulse.X -= 5;
            //}

            Physics.Update(ticksPassed, Services.Session.Level);
            Physics.Impulse = Vector.Zero;

            //if (Physics.HorizontalCollision.Collision)
            //{
            //    Character.Direction = Character.Direction == Direction.Right ? Direction.Left : Direction.Right;
            //}

            Character.Update(time, ticksPassed);
        }

        private void MoveToCover()
        {
        }

        private bool CanSeeTarget(Entity target)
        {
            var result = RayCaster.CastRay(Services.Session.Level, Location, Target.Location - Location, 500);
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

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location)
        {
            Physics.Impulse += vector.ToUnit() * 10;

            Character.HitByProjectile(projectile, vector, location);
        }
    }
}
