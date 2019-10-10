using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game.Levels
{
    public static class Loader
    {
        public static Level Load(string file)
        {
            var jsonData = File.ReadAllText(file, Encoding.UTF8);
            var levelData = FileFormats.Levels.Loader.Load(file);

            var level = new Level(levelData.Width, levelData.Height);
            for (int y = 0; y < levelData.CollisionMap.Height; y++)
            {
                for (int x = 0; x < levelData.CollisionMap.Width; x++)
                {
                    level.Cells[x, y] = levelData.CollisionMap.Data[y][x] ? BlockType.Block : BlockType.Open;
                }
            }

            return level;
        }
    }
}
