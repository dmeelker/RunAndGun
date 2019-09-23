using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using SdlTest.Types;

namespace SdlTest.Entities
{
    public class Crate : Entity, IProjectileCollider, IPhysicsCollider
    {
        private IntPtr textureId;
        private int hitpoint = 10;

        public Crate(Vector location)
        {
            Location = location;
            Size = new Vector(50, 50);
            textureId = Services.TextureManager["crate"];
        }

        public override void Render(IntPtr rendererId)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = (int) Size.X,
                h = (int) Size.Y
            };

            var destination = new SDL.SDL_Rect()
            {
                x = (int)Location.X,
                y = (int)Location.Y,
                w = (int)Size.X,
                h = (int)Size.Y
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);
        }

        public void HitByProjectile(Projectile projectile, Vector vector)
        {
            hitpoint--;
            if (hitpoint == 0)
                Dispose();
        }
    }
}
