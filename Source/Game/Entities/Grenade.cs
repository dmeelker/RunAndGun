using Game.Physics;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public class Grenade : Entity
    {
        public PhysicsComponent Physics;
        private Entity source;
        private Sprite sprite;
        private uint creationTime = 0;

        public Grenade(Entity source, Vector location, Vector velocity)
        {
            Physics = Services.Game.Physics.CreateComponent(this);
            Physics.Velocity = velocity;
            Physics.drag = 1;

            this.source = source;
            sprite = Services.Sprites["grenade"];

            Location = location;
            Size = new Vector(sprite.Width, sprite.Height);
            creationTime = Services.Time;
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Game.Level);

            var age = time - creationTime;
            if(age > 3000)
            {
                Explode();
            }
        }

        private void Explode()
        {
            Explosion.Create(Location, 50, 20);
            Dispose();
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, Location.ToPoint() - viewOffset);
        }

        public override void OnDisposed()
        {
            Services.Game.Physics.DisposeComponent(Physics);
        }
    }
}
