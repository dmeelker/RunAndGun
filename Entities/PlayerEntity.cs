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
        public PhysicsComponent Physics = new PhysicsComponent();
        private Level level;
        private IntPtr textureId;

        public PlayerEntity(IntPtr textureId, Level level, Vector location)
        {
            this.textureId = textureId;
            this.level = level;
            Physics.Location = location;
            Physics.Size = new Vector(30, 30);
            Physics.Velocity.Y = 0.5;
        }

        public override void Update(int ticksPassed)
        {
            Physics.Update(ticksPassed, level);
        }

        public override void Render(IntPtr rendererId)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = 30,
                h = 30
            };

            var destination = new SDL.SDL_Rect()
            {
                x = (int) Physics.Location.X,
                y = (int) Physics.Location.Y,
                w = 30,
                h = 30
            };

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);
        }
    }
}
