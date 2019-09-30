using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Levels
{
    public enum BlockType
    {
        Open,
        Block,
        StairLeft,
        StairRight
    }

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

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Cells[x, y] = BlockType.Open;

            for (int x = 0; x < width; x++)
            {
                Cells[x, 0] = BlockType.Block;
                Cells[x, height - 1] = BlockType.Block;
            }

            for (int y = 0; y < height; y++)
            {
                Cells[0, y] = BlockType.Block;
                Cells[width - 1,y] = BlockType.Block;
            }

            Cells[10, 16] = BlockType.Block;
            Cells[11, 16] = BlockType.Block;
            Cells[12, 16] = BlockType.Block;

            Cells[15, 13] = BlockType.Block;
            Cells[16, 13] = BlockType.Block;
            Cells[17, 13] = BlockType.Block;

            Cells[20, 19] = BlockType.Block;
            Cells[20, 18] = BlockType.Block;
            Cells[20, 17] = BlockType.Block;
            //Cells[30, 18] = BlockType.StairRight;
            //Cells[31, 17] = BlockType.StairRight;
            //Cells[32, 16] = BlockType.StairRight;
        }

        public BlockType IsPixelPassable(int x, int y)
        {
            if (x <= 0 || x >= WidthInPixels || y <= 0 || y >= HeightInPixels)
                return BlockType.Block;

            return Cells[x / BlockSize, y / BlockSize];
        }
    }
}
