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

        public Enemy(IntPtr textureId, IntPtr gunTexureId, Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, textureId, gunTexureId);

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(int ticksPassed)
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
                Character.Direction = Character.Direction == Direction.Right ? Direction.Left : Direction.Right;

            Character.Update(ticksPassed);
        }

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }

        public void HitByProjectile(Projectile projectile, Vector vector)
        {
            Physics.Impulse += vector.ToUnit() * 10;
        }
    }
}
