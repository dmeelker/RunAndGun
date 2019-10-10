using SharedTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Levels
{
    public class Level
    {
        public const int BlockSize = 20;
        public readonly int Width;
        public readonly int Height;
        public readonly int WidthInPixels;
        public readonly int HeightInPixels;
        public readonly BlockType[,] Cells;

        public Level(int width, int height)
        {
            Width = width;
            Height = height;
            WidthInPixels = width * BlockSize;
            HeightInPixels = height * BlockSize;
            Cells = new BlockType[width, height];
        }

        public BlockType GetBlockByPixelLocation(int x, int y)
        {
            if (x < 0 || x >= WidthInPixels || y < 0 || y >= HeightInPixels)
                return BlockType.Solid;

            return Cells[x / BlockSize, y / BlockSize];
        }

        public BlockType GetBlock(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return BlockType.Open;

            return Cells[x, y];
        }
    }
}
