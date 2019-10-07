using SdlTest.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public static class Services
    {
        public static readonly EntityManager EntityManager = new EntityManager();
        public static readonly TextureManager Textures = new TextureManager();
        public static readonly SpriteManager Sprites = new SpriteManager();
        public static readonly FontManager Fonts = new FontManager();
        public static GameSession Session;
        public static readonly Random Random = new Random();
        public static uint Time;
    }
}
