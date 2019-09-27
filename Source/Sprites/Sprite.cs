using SDL2;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest.Sprites
{
    public class Sprite
    {
        public readonly IntPtr TextureId;
        public readonly int Width;
        public readonly int Height;

        public Sprite(IntPtr textureId)
        {
            TextureId = textureId;
            SDL.SDL_QueryTexture(textureId, out _, out _, out var width, out var height);

            Width = width;
            Height = height;
        }

        public void Draw(IntPtr rendererId, int x, int y)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = Width,
                h = Height
            };

            var destination = new SDL.SDL_Rect()
            {
                x = x,
                y = y,
                w = Width,
                h = Height
            };

            SDL.SDL_RenderCopy(rendererId, TextureId, ref source, ref destination);
        }

        public void DrawEx(IntPtr rendererId, int x, int y, double angle, Vector? center, SDL.SDL_RendererFlip flipMode)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = Width,
                h = Height
            };

            var destination = new SDL.SDL_Rect()
            {
                x = x,
                y = y,
                w = Width,
                h = Height
            };
            
            if (center.HasValue)
            {
                var centerPoint = new SDL.SDL_Point { x = (int) center.Value.X, y = (int) center.Value.Y };
                SDL.SDL_RenderCopyEx(rendererId, TextureId, ref source, ref destination, angle, ref centerPoint, flipMode);
            }
            else
            {
                SDL.SDL_RenderCopyEx(rendererId, TextureId, ref source, ref destination, angle, IntPtr.Zero, flipMode);
            }
        }
    }
}
