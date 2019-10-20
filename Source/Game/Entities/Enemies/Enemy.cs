using Game.Components;
using Game.Entities.Collectables;
using Game.Levels;
using Game.Physics;
using Game.Types;
using Game.Utilities;
using Game.Weapons;
using SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Entities.Enemies
{
    public class Enemy : Entity, IProjectileCollider
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;

        public Entity Target;

        public int SenseRange = 400;
        public int Accuracy = 0;

        private TickTimer updateTimer = new TickTimer(100);
        private TickTimer fireTimer = new TickTimer(2000);

        public Enemy(Vector location, Direction initialDirection)
        {
            var sprite = Services.Sprites["player"];

            Physics = Services.Game.Physics.CreateComponent(this);
            Character = new CharacterComponent(this, sprite, new Pistol() { InfiniteAmmo = true });

            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);

            Character.FaceDirection(initialDirection);
        }

        public void SetWeapon(Weapon weapon)
        {
            weapon.InfiniteAmmo = true;
            Character.Weapon = weapon;
        }

        public override void Update(uint time, int ticksPassed)
        {
            if (updateTimer.Update(time))
            {
                UpdateTarget(time);
            }

            if (Target != null && CanSeeTarget(Target))
            {
                if (!InFiringRange(Target))
                {
                    MoveTowardsTarget();
                }
                else
                {
                    Character.AimAt(Target.Location.ToPoint());
                    if (Character.Weapon.ReloadNeeded)
                    {
                        MoveToCover();
                        Character.Weapon.Reload(time);
                    }
                    else
                    {
                        if (fireTimer.Update(time))
                        {
                            Character.Fire(time, Accuracy);
                        }
                    }
                }
            }

            Physics.Update(ticksPassed, Services.Game.Level);
            Physics.Impulse = Vector.Zero;

            Character.Update(time, ticksPassed);
        }

        private void UpdateTarget(uint time)
        {
            var newTarget = Services.Game.Entities.FindEntities(new Rect(Location.X - SenseRange, Location.Y - SenseRange, SenseRange * 2, SenseRange * 2)).Where(entity => entity is PlayerEntity).FirstOrDefault();

            if (Target == null && newTarget != null && FacingTowardsTarget(newTarget) && CanSeeTarget(newTarget))
            {
                Target = newTarget;
            }
        }

        private void MoveTowardsTarget()
        {
            if(Target.Location.X > Location.X)
            {
                // Right
                Physics.Impulse += new Vector(4, 0);
                Character.FaceDirection(Direction.Right);
            } 
            else
            {
                // Left
                Physics.Impulse += new Vector(-4, 0);
                Character.FaceDirection(Direction.Left);
            }
        }

        private void MoveToCover()
        {
        }

        private bool CanSeeTarget(Entity target)
        {
            var vector = target.Location - Location;
            var maxDistance = (int)Math.Min(vector.Length, 500);

            var result = RayCaster.CastRay(Services.Game.Level, Location, vector, CollisionCheckType.BlocksProjectiles, maxDistance);
            return !result.Hit;
        }

        private bool InFiringRange(Entity target)
        {
            var distance = DistanceTo(target);
            return distance <= Character.Weapon.EffectiveRange && distance <= SenseRange;
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

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location, Entity source)
        {
            Physics.Impulse += vector.ToUnit() * 10;

            Character.HitByProjectile(projectile, vector, location);

            if(Target == null)
            {
                if (source.Location.X > Location.X)
                    Character.FaceDirection(Direction.Right);
                else if (source.Location.X < Location.X)
                    Character.FaceDirection(Direction.Left);
            }
        }

        public override void OnDisposed()
        {
            Services.Game.Physics.DisposeComponent(Physics);
        }
    }
}
