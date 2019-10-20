using SDL2;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Game.Physics;

namespace Game.Entities
{
    public class Wreckage : Entity
    {
        private Sprite sprite;
        private Vector creationLocation;
        private Vector velocity;
        private int age = 0;
        private PhysicsComponent Physics;
        public Wreckage(Vector location, Vector vector)
        {
            Physics = Services.Game.Physics.CreateComponent(this);
            Physics.Velocity = vector;
            Physics.drag = .5;

            creationLocation = location;
            Location = location;
            Size = new Vector(10, 10);
            velocity = vector;
            sprite = Services.Sprites["crate"];
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Game.Level);
            age += ticksPassed;
            //velocity.Y += 1 * (ticksPassed * PhysicsComponent.tickMultiplier);
            //velocity.Y = Math.Min(velocity.Y, 15);

            //Location += velocity * (ticksPassed * PhysicsComponent.tickMultiplier);

            if (age > 2000)
                Dispose();
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
