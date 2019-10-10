using SDL2;
using Game.Sprites;
using Game.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class FontManager
    {
        private readonly Dictionary<string, Font> fonts = new Dictionary<string, Font>();

        public void Add(Font sprite, string key)
        {
            fonts.Add(key, sprite);
        }

        public Font this[string key] => fonts[key];
    }
}
