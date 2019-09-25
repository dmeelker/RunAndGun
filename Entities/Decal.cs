using SDL2;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Decal : Entity
    {
        private IntPtr textureId;

        public Decal(Vector location)
        {
            textureId = Services.TextureManager["floor-blood"];
            Location = location;
            Size = new Vector(8, 4);
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

            SDL.SDL_RenderCopy(rendererId, textureId, ref source, ref destination);
        }
    }
}
