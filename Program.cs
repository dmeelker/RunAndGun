using SDL2;
using SdlTest.Entities;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Runtime.InteropServices;

namespace SdlTest
{
    class Program
    {
        static IntPtr blockTex;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            var win = SDL.SDL_CreateWindow(".NET Core SDL2-CS Tutorial",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                1028,
                800,
                SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE
            );

            var ren = SDL.SDL_CreateRenderer(win, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            if (ren == null)
            {
                SDL.SDL_DestroyWindow(win);
                Console.WriteLine($"SDL_CreateRenderer Error: {SDL.SDL_GetError()}");
                SDL.SDL_Quit();
                return;
            }

            var tex = SDL_image.IMG_LoadTexture(ren, "res/test.png");
            if (tex == IntPtr.Zero)
            {
                SDL.SDL_DestroyRenderer(ren);
                SDL.SDL_DestroyWindow(win);
                Console.WriteLine($"SDL_CreateTextureFromSurface Error: {SDL.SDL_GetError()}");
                SDL.SDL_Quit();
                return;
            }

            blockTex = SDL_image.IMG_LoadTexture(ren, "res/block.png");
            if (tex == IntPtr.Zero)
            {
                SDL.SDL_DestroyRenderer(ren);
                SDL.SDL_DestroyWindow(win);
                Console.WriteLine($"SDL_CreateTextureFromSurface Error: {SDL.SDL_GetError()}");
                SDL.SDL_Quit();
                return;
            }

            var source = new SDL.SDL_Rect() {
                x = 0,
                y = 0,
                w = 30,
                h = 30
            };
            var destination = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = 30,
                h = 30
            };

            var level = new Level(20, 20);
            var entities = new EntityManager();
            var player = new PlayerEntity(tex, level, new Vector(30, 30));

            entities.Add(player);
            var quit = false;

            while (!quit)
            {
                while (SDL.SDL_PollEvent(out var e) > 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quit = true;
                    }
                    
                    if(e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_LEFT)
                            player.Physics.Impulse.X = -10;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                            player.Physics.Impulse.X = 10;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP)
                            player.Physics.Velocity.Y = -10;
                        //else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_DOWN)
                        //    destination.y += 10;
                    } else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_LEFT)
                            player.Physics.Impulse.X = 0;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                            player.Physics.Impulse.X = 0;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP)
                            destination.y -= 5;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_DOWN)
                            destination.y += 5;
                    }
                }

                entities.UpdateEntities(1);

                SDL.SDL_RenderClear(ren);

                RenderLevel(ren, level);

                entities.RenderEntities(ren);

                //SDL.SDL_RenderCopy(ren, tex, ref source, ref destination);
                SDL.SDL_RenderPresent(ren);
            }

            SDL.SDL_DestroyTexture(tex);
            SDL.SDL_DestroyTexture(blockTex);
            SDL.SDL_DestroyRenderer(ren);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_Quit();
        }

        private static void RenderLevel(IntPtr ren, Level level)
        {
            var source = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = 30,
                h = 30
            };

            var rect = new SDL.SDL_Rect()
            {
                x = 0,
                y = 0,
                w = Level.BlockSize,
                h = Level.BlockSize
            };

            for (int x=0; x<level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    rect.x = x * Level.BlockSize;
                    rect.y = y * Level.BlockSize;

                    if(!level.Cells[x, y])
                        SDL.SDL_RenderCopy(ren, blockTex, ref source, ref rect);
                }
            }
        }
    }
}
