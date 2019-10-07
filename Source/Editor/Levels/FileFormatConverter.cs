using FileFormats.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Levels
{
    public static class FileFormatConverter
    {
        public static LevelFile ConvertToFileFormat(Level level)
        {
            return new LevelFile { 
                Width = level.Width,
                Height = level.Height,
                ImageName = "backdrop.png",
                CollisionMap = ConvertCollisionMap(level.CollisionMap)
            };
        }

        public static Level ConvertFromFileFormat(LevelFile levelData)
        {
            var level = new Level(levelData.Width, levelData.Height);

            for (int y = 0; y < levelData.CollisionMap.Height; y++)
            {
                for (int x = 0; x < levelData.CollisionMap.Width; x++)
                {
                    level.CollisionMap[x, y] = levelData.CollisionMap.Data[y][x];
                }
            }

            return level;
        }

        private static LevelFile.CollisionMapSection ConvertCollisionMap(CollisionMap collisionMap)
        {
            return new LevelFile.CollisionMapSection()
            {
                Width = collisionMap.Width,
                Height = collisionMap.Height,
                Data = ConvertCollisionMapData(collisionMap)
            };
        }

        private static bool[][] ConvertCollisionMapData(CollisionMap collisionMap)
        {
            var data = new bool[collisionMap.Height][];

            for(int y=0; y<collisionMap.Height; y++)
            {
                data[y] = new bool[collisionMap.Width];

                for(int x=0; x<collisionMap.Width; x++)
                {
                    data[y][x] = collisionMap[x, y];
                }
            }

            return data;
        }
    }
}
