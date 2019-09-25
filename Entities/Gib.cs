using SDL2;
using SdlTest.Components;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Gib : Entity
    {
        public PhysicsComponent Physics;
        private IntPtr textureId;
        private readonly Vector creationLocation;

        public Gib(Vector location, Vector vector)
        {
            Physics = new PhysicsComponent(this) { 
                Velocity = vector
            };

            textureId = Services.TextureManager["gib"];
            creationLocation = location;
            Location = location;
            Size = new Vector(8, 8);
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);

            if(Physics.VerticalCollision.Collision)
            {
                var bloodDecal = new Decal(new Vector(Physics.VerticalCollision.X - (Size.X / 2), Physics.VerticalCollision.Y));
                Services.EntityManager.Add(bloodDecal);
                Dispose();
            }
        }

        public override void Render(IntPtr rendererId)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = (int)Size.X,
                h = (int)Size.Y
            };

            var destination = new SDL.SDL_Rect()
            {
                x = (int)Location.X,
                y = (int)Location.Y,
                w = (int)Size.X,
                h = (int)Size.Y
            };

            var angle = (Location.X - creationLocation.X) * 2.8;

            SDL.SDL_RenderCopyEx(rendererId, textureId, ref source, ref destination, angle, IntPtr.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
