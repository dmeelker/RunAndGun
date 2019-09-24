using SdlTest.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public static class Services
    {
        public static readonly EntityManager EntityManager = new EntityManager();
        public static readonly TextureManager TextureManager = new TextureManager();
        public static GameSession Session;
        public static readonly Random Random = new Random();
    }
}
