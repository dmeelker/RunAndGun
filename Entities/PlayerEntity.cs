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

        public PlayerEntity(IntPtr textureId, IntPtr gunTexureId, Vector location)
        {
            Physics = new PhysicsComponent(this);
            Character = new CharacterComponent(this, textureId, gunTexureId);

            Location = location;
            Size = new Vector(30, 30);
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, Services.Session.Level);
            Character.Update(ticksPassed);
        }

        public void AimAt(int x, int y)
        {
            Character.AimAt(x, y);
        }

        public void Fire()
        {
            Character.Fire();
        }

        public override void Render(IntPtr rendererId)
        {
            Character.Render(rendererId);
        }
    }
}
