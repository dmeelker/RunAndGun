using System;
using System.Collections.Generic;
using System.Text;

namespace FileFormats.Levels
{
    public class LevelFile
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImageName { get; set; }
        public CollisionMapSection CollisionMap { get; set; }

        public class CollisionMapSection
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public bool[][] Data { get; set; }
        }
    }
}
