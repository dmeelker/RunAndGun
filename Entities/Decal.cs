using SDL2;
using SdlTest.Sprites;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Entities
{
    public class Decal : Entity
    {
        private Sprite sprite;

        public Decal(Vector location)
        {
            sprite = Services.SpriteManager["floor-blood"];
            Location = location;
            Size = new Vector(8, 4);
        }

        public override void Render(IntPtr rendererId)
        {
            sprite.Draw(rendererId, (int)Location.X, (int)Location.Y);
        }
    }
}
