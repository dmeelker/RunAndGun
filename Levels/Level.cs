using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Levels
{
    public class Level
    {
        public const int BlockSize = 20;
        public readonly int Width;
        public readonly int Height;
        public readonly int WidthInPixels;
        public readonly int HeightInPixels;
        public readonly bool[,] Cells;

        public Level(int width, int height)
        {
            Width = width;
            Height = height;
            WidthInPixels = width * BlockSize;
            HeightInPixels = height * BlockSize;
            Cells = new bool[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Cells[x, y] = true;

            for (int x = 0; x < width; x++)
            {
                Cells[x, 0] = false;
                Cells[x, height - 1] = false;
            }

            for (int y = 0; y < height; y++)
            {
                Cells[0, y] = false;
                Cells[width - 1,y] = false;
            }

            Cells[10, 16] = false;
            Cells[11, 16] = false;
            Cells[12, 16] = false;

            Cells[15, 13] = false;
            Cells[16, 13] = false;
            Cells[17, 13] = false;
        }

        public bool IsPixelPassable(int x, int y)
        {
            if (x <= 0 || x >= WidthInPixels || y <= 0 || y >= HeightInPixels)
                return false;

            return Cells[x / BlockSize, y / BlockSize];
        }
    }
}
