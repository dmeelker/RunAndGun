using SharedTypes;
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
        public Enemy[] Enemies { get; set; }

        public class CollisionMapSection
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public BlockType[][] Data { get; set; }
        }

        public class Enemy
        {
            public int X { get; set; }
            public int Y { get; set; }
            public EnemyType Type { get; set; }
            public Direction Direction { get; set; }
        }
    }
}
