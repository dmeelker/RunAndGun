using SDL2;
using System;
using System.Runtime.InteropServices;

namespace SdlTest
{
    class Program
    {
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
            //SDL_image.IMG_LoadTexture(ren, "res/test.png");
            //var bmp = SDL.SDL_LoadBMP("res/test.png");
            //SDL.SDL_Load
            //if (bmp == IntPtr.Zero)
            //{
            //    SDL.SDL_DestroyRenderer(ren);
            //    SDL.SDL_DestroyWindow(win);
            //    Console.WriteLine($"SDL_LoadBMP Error: {SDL.SDL_GetError()}");
            //    SDL.SDL_Quit();
            //    return;
            //}
            //var memory = Marshal.AllocHGlobal(1000);
            //Marshal.Release(memory);

            //SDL.SDL_RWFromMem()
            //SDL_image.IMG_Load_RW
            var tex = SDL_image.IMG_LoadTexture(ren, "res/test.png");
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
                            destination.x -= 5;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                            destination.x += 5;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP)
                            destination.y -= 5;
                        else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_DOWN)
                            destination.y += 5;
                    }
                }

                SDL.SDL_RenderClear(ren);
                SDL.SDL_RenderCopy(ren, tex, ref source, ref destination);
                SDL.SDL_RenderPresent(ren);
            }

            SDL.SDL_DestroyTexture(tex);
            SDL.SDL_DestroyRenderer(ren);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_Quit();
        }
    }
}
