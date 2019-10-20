using SDL2;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Game.Physics;

namespace Game.Entities
{
    public class Gib : Entity
    {
        public PhysicsComponent Physics;
        private Sprite sprite;
        private readonly Vector creationLocation;

        public Gib(Vector location, Vector vector)
        {
            Physics = Services.Game.Physics.CreateComponent(this);
            Physics.Velocity = vector;

            sprite = Services.Sprites["gib"];
            creationLocation = location;
            Location = location;
            Size = new Vector(8, 8);
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Game.Level);

            if(Physics.VerticalCollision.Collision)
            {
                var bloodDecal = new Decal(new Vector(Physics.VerticalCollision.X - (Size.X / 2), Physics.VerticalCollision.Y));
                Services.Game.Entities.Add(bloodDecal);
                Dispose();
            }
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            var angle = (Location.X - creationLocation.X) * 2.8;

            sprite.DrawEx(rendererId, Location.ToPoint() - viewOffset, angle, null, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public override void OnDisposed()
        {
            Services.Game.Physics.DisposeComponent(Physics);
        }
    }
}
