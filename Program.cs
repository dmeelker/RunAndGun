using SDL2;
using SdlTest.Entities;
using SdlTest.Levels;
using SdlTest.Types;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace SdlTest
{
    class Program
    {
        static int fps;
        static int fpsCounter;
        static uint lastFpsUpdateTime = SDL.SDL_GetTicks();

        static IntPtr win;
        static IntPtr ren;

        static PlayerEntity player;

        static bool quit = false;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            win = SDL.SDL_CreateWindow(".NET Core SDL2-CS Tutorial",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                800,
                600,
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN
            );

            ren = SDL.SDL_CreateRenderer(win, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
            if (ren == null)
            {
                SDL.SDL_DestroyWindow(win);
                Console.WriteLine($"SDL_CreateRenderer Error: {SDL.SDL_GetError()}");
                SDL.SDL_Quit();
                return;
            }

            LoadTextures();

            Services.Session = new GameSession() {
                Level = new Level(40, 20)
            };

            player = new PlayerEntity(Services.TextureManager["player"], Services.TextureManager["shotgun"], new Vector(30, 30));
            Services.EntityManager.Add(player);

            var enemy = new Enemy(Services.TextureManager["player"], Services.TextureManager["shotgun"], new Vector(230, 30));
            Services.EntityManager.Add(enemy);

            Services.EntityManager.Add(new Crate(new Vector(400, 330)));

            uint lastUpdateTime = SDL.SDL_GetTicks();

            while (!quit)
            {
                var time = SDL.SDL_GetTicks();
                var timePassed = (int)(time - lastUpdateTime);
                lastUpdateTime = time;

                Update(timePassed);
                Render(time);
            }

            Services.TextureManager.Cleanup();
            SDL.SDL_DestroyRenderer(ren);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_Quit();
        }

        private static void LoadTextures()
        {
            Services.TextureManager.LoadTexture(ren, "res/test.png", "player");
            Services.TextureManager.LoadTexture(ren, "res/block.png", "block");
            Services.TextureManager.LoadTexture(ren, "res/projectile.png", "projectile");
            Services.TextureManager.LoadTexture(ren, "res/shotgun.png", "shotgun");
            Services.TextureManager.LoadTexture(ren, "res/crate.png", "crate");
        }

        private static void Update(int timePassed)
        {
            while (SDL.SDL_PollEvent(out var e) > 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    quit = true;
                }

                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_LEFT)
                        player.Physics.Impulse.X = -10;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                        player.Physics.Impulse.X = 10;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_UP)
                        player.Physics.Velocity.Y = -13;

                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_LEFT)
                        player.Physics.Impulse.X = 0;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_RIGHT)
                        player.Physics.Impulse.X = 0;
                }
                else if(e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    player.Fire();
                }
            }

            SDL.SDL_GetMouseState(out var x, out var y);
            player.AimAt(x, y);

            Services.EntityManager.UpdateEntities(timePassed);
        }

        private static void Render(uint time)
        {
            SDL.SDL_RenderClear(ren);

            RenderLevel(ren, Services.Session.Level);
            Services.EntityManager.RenderEntities(ren);

            SDL.SDL_RenderPresent(ren);
            fpsCounter++;

            if (time - lastFpsUpdateTime >= 1000)
            {
                fps = fpsCounter;
                fpsCounter = 0;
                lastFpsUpdateTime = time;
                SDL.SDL_SetWindowTitle(win, "FPS: " + fps);
            }
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

            var blockTexture = Services.TextureManager["block"];

            for (int x=0; x<level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    rect.x = x * Level.BlockSize;
                    rect.y = y * Level.BlockSize;

                    if(!level.Cells[x, y])
                        SDL.SDL_RenderCopy(ren, blockTexture, ref source, ref rect);
                }
            }
        }
    }
}
