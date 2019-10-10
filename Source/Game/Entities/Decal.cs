using SDL2;
using Game.Sprites;
using Game.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Entities
{
    public class Decal : Entity
    {
        private Sprite sprite;

        public Decal(Vector location)
        {
            sprite = Services.Sprites["floor-blood"];
            Location = location;
            Size = new Vector(8, 4);
        }

        public override void Render(IntPtr rendererId, Point viewOffset)
        {
            sprite.Draw(rendererId, Location.ToPoint() - viewOffset);
        }
    }
}
