using SDL2;
using SdlTest.Components;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class PlayerEntity : Entity
    {
        public readonly PhysicsComponent Physics;
        public readonly CharacterComponent Character;

        public PlayerEntity(Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, Services.SpriteManager["player"], new Shotgun());

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(uint time, int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);
            Character.Update(time, ticksPassed);
        }

        public void AimAt(int x, int y)
        {
            Character.AimAt(x, y);
        }

        public void Fire(uint time)
        {
            Character.Fire(time);
        }

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }
    }
}
