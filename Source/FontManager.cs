using SDL2;
using SdlTest.Sprites;
using SdlTest.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
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
