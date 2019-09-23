using SDL2;
using SdlTest.Components;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SdlTest.Entities
{
    public class Projectile : Entity
    {
        private Entity source;
        public PhysicsComponent Physics;
        private IntPtr textureId;

        public Projectile(Entity source, IntPtr textureId, Vector location, Vector velocity)
        {
            this.textureId = textureId;
            this.source = source;
            Physics = new PhysicsComponent(this);

            Location = location;
            Size = new Vector(8, 8);
            Physics.Velocity = velocity;
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);

            if (Physics.HorizontalCollision.Collision || Physics.VerticalCollision.Collision)
            {
                Dispose();
                return;
            }

            var entityCollisions = Services.EntityManager.FindEntities(GetBoundingBox()).ToArray();

            foreach(var entity in entityCollisions)
            {
                if (entity == source || entity == this)
                    continue;

                if(entity is IProjectileCollider)
                {
                    ((IProjectileCollider)entity).HitByProjectile(this, Physics.Velocity);
                    Dispose();
                    break;
                }
            }
        }

        public override void Render(IntPtr rendererId)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = 8,
                h = 8
            };

            var destination = new SDL.SDL_Rect()
            {
                x = (int)Location.X,
                y = (int)Location.Y,
                w = 8,
                h = 8
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);
        }
    }
}
