using SDL2;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Game.Physics;

namespace Game.Entities
{
    public class BulletCasing : Entity
    {
        public PhysicsComponent Physics;
        private Sprite sprite;
        private readonly Vector creationLocation;
        private int age = 0;

        public BulletCasing(Vector location, Vector vector, Sprite sprite)
        {
            Physics = Services.Game.Physics.CreateComponent(this);
            Physics.Velocity = vector;
            Physics.drag = 1;

            this.sprite = sprite;
            creationLocation = location;
            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Game.Level);

            age += ticksPassed;
            if (age > 10000)
                Dispose();
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            var angle = (Location.X - creationLocation.X) * 15;

            sprite.DrawEx(rendererId, Location.ToPoint() - viewOffset, angle, null, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public override void OnDisposed()
        {
            Services.Game.Physics.DisposeComponent(Physics);
        }
    }
}
