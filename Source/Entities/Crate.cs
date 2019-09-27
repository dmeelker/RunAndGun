using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using SdlTest.Sprites;
using SdlTest.Types;

namespace SdlTest.Entities
{
    public class Crate : Entity, IProjectileCollider, IPhysicsCollider
    {
        private Sprite sprite;
        private int hitpoint = 10;

        public Crate(Vector location)
        {
            Location = location;
            Size = new Vector(50, 50);
            sprite = Services.SpriteManager["crate"];
        }

        public override void Render(IntPtr rendererId)
        {
            sprite.Draw(rendererId, (int)Location.X, (int)Location.Y);
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location)
        {
            var offset = (vector.ToUnit() * -1) * 10;
            vector = (vector.ToUnit() * -1) * Services.Random.Next(2, 5);
            
            var wreckage = new Wreckage(Location + location + offset, vector);
            Services.EntityManager.Add(wreckage);

            //hitpoint--;
            //if (hitpoint == 0)
            //    Dispose();
        }
    }
}
