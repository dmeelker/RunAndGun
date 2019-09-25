using SDL2;
using SdlTest.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public class SpriteManager
    {
        private readonly Dictionary<string, Sprite> textures = new Dictionary<string, Sprite>();

        public void Add(Sprite sprite, string key)
        {
            textures.Add(key, sprite);
        }

        public Sprite this[string key] => textures[key];
    }
}
