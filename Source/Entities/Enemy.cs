using SdlTest.Components;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Enemy : Entity, IProjectileCollider
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;
        public int Hitpoints = 10;

        public Enemy(Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, Services.SpriteManager["player"], new Pistol());

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(uint time, int ticksPassed)
        {
            if (Character.Direction == Direction.Right)
            {
                Physics.Impulse.X += 5;
            } 
            else if (Character.Direction == Direction.Left)
            {
                Physics.Impulse.X -= 5;
            }

            Physics.Update(ticksPassed, Services.Session.Level);
            Physics.Impulse = Vector.Zero;

            if (Physics.HorizontalCollision.Collision)
            {
                Character.Direction = Character.Direction == Direction.Right ? Direction.Left : Direction.Right;
            }

            Character.Update(time, ticksPassed);
        }

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location)
        {
            Physics.Impulse += vector.ToUnit() * 10;

            Hitpoints -= projectile.Power;

            SpawnGibs(vector, location);

            if (Hitpoints <= 0)
                Die();
        }

        private void SpawnGibs(Vector vector, Vector location)
        {
            var count = Services.Random.Next(2, 5);

            for (var i = 0; i < count; i++)
            {
                vector = vector.ToUnit() * Services.Random.Next(2, 5);
                Services.EntityManager.Add(new Gib(Location + location, vector));
            }
        }

        private void Die()
        {
            for (var i = 0; i < 100; i++)
            {
                var angle = Services.Random.Next(0, 360) / (180.0 / Math.PI);
                var vector = new Vector(Math.Sin(angle), Math.Cos(angle));
                var power = Services.Random.Next(5, 10);
                
                Services.EntityManager.Add(new Gib(Location + (Size * 0.5), vector * power));
            }

            Dispose();
        }
    }
}
