﻿using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using Game.Sprites;
using Game.Types;

namespace Game.Entities
{
    public class Crate : Entity, IProjectileCollider, IPhysicsCollider
    {
        private Sprite sprite;
        private int hitpoint = 10;

        public Crate(Vector location)
        {
            Location = location;
            Size = new Vector(50, 50);
            sprite = Services.Sprites["crate"];
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, Location.ToPoint() - viewOffset);
        }

        public void HitByProjectile(Projectile projectile, Vector vector, Vector location, Entity source)
        {
            var offset = (vector.ToUnit() * -1) * 10;
            vector = (vector.ToUnit() * -1) * Services.Random.Next(2, 5);
            
            var wreckage = new Wreckage(Location + location + offset, vector);
            Services.Game.Entities.Add(wreckage);

            hitpoint -= projectile.Power;
            if (hitpoint <= 0)
                Dispose();
        }
    }
}
