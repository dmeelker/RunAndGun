using SharedTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Levels
{
    public class Level
    {
        public const int BlockSize = 20;

        public int Width { get; set; }
        public int Height { get; set; }
        public Image Image { get; set; }

        public CollisionMap CollisionMap { get; private set; }
        public List<Enemy> Enemies { get; private set; } = new List<Enemy>();

        public Level(int width, int height)
        {
            Width = width;
            Height = height;
            CollisionMap = new CollisionMap(width / BlockSize, height / BlockSize);
        }
    }

    public class CollisionMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private readonly BlockType[,] data;
        public BlockType[,] Data => data;

        public CollisionMap(int width, int height)
        {
            Width = width;
            Height = height;
            data = new BlockType[width, height];
        }

        public BlockType this[int x, int y]
        {
            get 
            {
                if (!ContainsPoint(x, y))
                    return BlockType.Solid;

                return data[x, y]; 
            }
            set 
            {
                if (!ContainsPoint(x, y))
                    return;

                data[x, y] = value; 
            }
        }

        private bool ContainsPoint(int x, int y)
        {
            return !(x < 0 || x >= Width || y < 0 || y >= Height);
        }
    }
}
