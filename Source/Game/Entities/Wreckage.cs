using SDL2;
using Game.Components;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public class Wreckage : Entity
    {
        private Sprite sprite;
        private Vector creationLocation;
        private Vector velocity;
        private int age = 0;
        private PhysicsComponent physics;
        public Wreckage(Vector location, Vector vector)
        {
            physics = new PhysicsComponent(this);
            physics.Velocity = vector;
            physics.drag = .5;

            creationLocation = location;
            Location = location;
            Size = new Vector(10, 10);
            velocity = vector;
            sprite = Services.Sprites["crate"];
        }

        public override void Update(uint time, int ticksPassed)
        {
            physics.Update(ticksPassed, Services.Game.Level);
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
    }
}
