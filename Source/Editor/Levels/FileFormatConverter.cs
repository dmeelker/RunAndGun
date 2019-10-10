using FileFormats.Levels;
using SharedTypes;
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
                CollisionMap = ConvertCollisionMap(level.CollisionMap),
                Enemies = level.Enemies.Select(enemy => new LevelFile.Enemy {
                    X = enemy.X,
                    Y = enemy.Y,
                    Type = enemy.Type,
                    Direction = enemy.Direction
                }).ToArray()
            };
        }

        public static Level ConvertFromFileFormat(LevelFile levelData)
        {
            var level = new Level(levelData.Width, levelData.Height);
            level.Enemies.AddRange(levelData.Enemies.Select(enemy => new Enemy {
                X = enemy.X,
                Y = enemy.Y,
                Type = enemy.Type,
                Direction = enemy.Direction
            }));

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

        private static BlockType[][] ConvertCollisionMapData(CollisionMap collisionMap)
        {
            var data = new BlockType[collisionMap.Height][];

            for(int y=0; y<collisionMap.Height; y++)
            {
                data[y] = new BlockType[collisionMap.Width];

                for(int x=0; x<collisionMap.Width; x++)
                {
                    data[y][x] = collisionMap[x, y];
                }
            }

            return data;
        }
    }
}
