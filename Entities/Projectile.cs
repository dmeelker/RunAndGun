using SDL2;
using SdlTest.Components;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Projectile : Entity
    {
        public PhysicsComponent Physics = new PhysicsComponent();
        private IntPtr textureId;

        public Projectile(IntPtr textureId, Vector location, Vector velocity)
        {
            this.textureId = textureId;
            Physics.Location = location;
            Physics.Size = new Vector(8, 8);
            Physics.Velocity = velocity;
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);
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
                x = (int)Physics.Location.X,
                y = (int)Physics.Location.Y,
                w = 8,
                h = 8
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);
        }
    }
}
