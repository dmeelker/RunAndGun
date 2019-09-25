using SDL2;
using SdlTest.Components;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Gib : Entity
    {
        public PhysicsComponent Physics;
        private Sprite sprite;
        private readonly Vector creationLocation;

        public Gib(Vector location, Vector vector)
        {
            Physics = new PhysicsComponent(this) { 
                Velocity = vector
            };

            sprite = Services.SpriteManager["gib"];
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
            var angle = (Location.X - creationLocation.X) * 2.8;

            sprite.DrawEx(rendererId, (int)Location.X, (int)Location.Y, angle, null, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
