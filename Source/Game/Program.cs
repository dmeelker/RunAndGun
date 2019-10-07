using SDL2;
using SdlTest.Components;
using SdlTest.Entities;
using SdlTest.Entities.Collectables;
using SdlTest.Levels;
using SdlTest.Text;
using SdlTest.Types;
using SdlTest.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
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
        static Point viewOffset;
        static Point viewSize = new Point(WindowWidth, WindowHeight);
        static Point viewSizeHalf = new Point(WindowWidth / 2, WindowHeight / 2);

        const int WindowWidth = 800;
        const int WindowHeight = 600;

        static void Main(string[] args)
        {
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            win = SDL.SDL_CreateWindow(".NET Core SDL2-CS Tutorial",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                WindowWidth,
                WindowHeight,
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

            player = new PlayerEntity(new Vector(30, 30));
            player.AddWeapon(new Pistol());
            Services.EntityManager.Add(player);

            //Services.EntityManager.Add(new Enemy(new Vector(230, 30)));
            //Services.EntityManager.Add(new Enemy(new Vector(100, 30)));
            Services.EntityManager.Add(new Enemy(new Vector(500, 300)));

            Services.EntityManager.Add(new Crate(new Vector(600, 330)));

            Services.EntityManager.Add(new WeaponCollectable(WeaponType.Shotgun, new Vector(300, 330)));

            
            uint lastUpdateTime = SDL.SDL_GetTicks();

            while (!quit)
            {
                var time = SDL.SDL_GetTicks();
                var timePassed = (int)(time - lastUpdateTime);
                lastUpdateTime = time;

                Update(time, timePassed);
                Render(time);
            }

            Services.Textures.Cleanup();
            SDL.SDL_DestroyRenderer(ren);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_Quit();
        }

        private static void LoadTextures()
        {
            Services.Textures.LoadTexture(ren, "res/test.png", "player");
            Services.Textures.LoadTexture(ren, "res/block.png", "block");
            Services.Textures.LoadTexture(ren, "res/projectile.png", "projectile");
            Services.Textures.LoadTexture(ren, "res/shotgun.png", "shotgun");
            Services.Textures.LoadTexture(ren, "res/crate.png", "crate");
            Services.Textures.LoadTexture(ren, "res/gib.png", "gib");
            Services.Textures.LoadTexture(ren, "res/floor-blood.png", "floor-blood");
            Services.Textures.LoadTexture(ren, "res/Font/DTM-Sans_0.png", "DTM-Sans_0");
            Services.Textures.LoadTexture(ren, "res/backdrop.png", "backdrop");

            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["player"]), "player");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["block"]), "block");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["projectile"]), "projectile");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["shotgun"]), "shotgun");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["crate"]), "crate");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["gib"]), "gib");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["floor-blood"]), "floor-blood");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["backdrop"]), "backdrop");

            using var fontFile = File.OpenRead(Path.Combine("res", "font", "DTM-Sans.fnt"));
            var font = new Font(fontFile, Services.Textures["DTM-Sans_0"]);
            Services.Fonts.Add(font, "default");
        }

        private static void Update(uint time, int timePassed)
        {
            while (SDL.SDL_PollEvent(out var e) > 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                {
                    quit = true;
                }

                if (e.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a)
                        player.Physics.Impulse.X = -10;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                        player.Physics.Impulse.X = 10;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_w)
                        player.Physics.Velocity.Y = -13;

                }
                else if (e.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_a)
                        player.Physics.Impulse.X = 0;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_d)
                        player.Physics.Impulse.X = 0;
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_r)
                        player.Character.Reload(time);
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_1)
                        player.ChangeWeapon(player.WeaponOrder[0]);
                    else if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_2)
                        player.ChangeWeapon(player.WeaponOrder[1]);
                }
                else if(e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    player.Fire(time);
                }
            }

            SDL.SDL_GetMouseState(out var x, out var y);
            player.AimAt(new Point(x, y) + viewOffset);

            Services.EntityManager.UpdateEntities(time, timePassed);

            CenterViewOnPlayer();
        }

        private static void CenterViewOnPlayer()
        {
            viewOffset = player.Location.ToPoint() - viewSizeHalf;
            viewOffset.Y = Math.Min(viewOffset.Y, Services.Session.Level.HeightInPixels - WindowHeight);
        }

        private static void Render(uint time)
        {
            SDL.SDL_SetRenderDrawColor(ren, 0, 0, 0, 0);
            SDL.SDL_RenderClear(ren);

            //Services.Sprites["backdrop"].Draw(ren, new Point(0, 0));

            RenderLevel(ren, Services.Session.Level);
            Services.EntityManager.RenderEntities(ren, viewOffset);

            //CastRay(player.Location + player.Character.WeaponOffset, player.Character.AimVector);

            var font = Services.Fonts["default"];
            var weapon = player.Character.Weapon;

            font.Render(ren, weapon.Name, 700, 500);
            font.Render(ren, $"{weapon.ClipContent} / {weapon.AmmoReserve}", 700, 520);

            font.Render(ren, $"HP: {player.Character.Hitpoints}/{CharacterComponent.MaxHitpoints}", 0, 500);
            font.Render(ren, $"Armor: {player.Character.Armor}/{CharacterComponent.MaxArmor}", 0, 520);

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

        private static void CastRay(Vector location, Vector vector, int maxDistance = 1000)
        {
            SDL.SDL_SetRenderDrawColor(ren, 255, 0, 0, 0);

            vector = vector.ToUnit() * 10;

            var currentLocation = location;
            var step = 0;
            while(step < maxDistance / 10)
            {
                step++;
                currentLocation += vector;
                var mapX = (int)(currentLocation.X / Level.BlockSize);
                var mapY = (int)(currentLocation.Y / Level.BlockSize);

                if(Services.Session.Level.IsPixelPassable((int)currentLocation.X, (int)currentLocation.Y) == BlockType.Block)
                {
                    SDL.SDL_RenderDrawLine(ren, (int)location.X, (int)location.Y, (int)(currentLocation.X), (int)(currentLocation.Y));
                    break;
                }
            }

            //SDL.SDL_RenderDrawLine(ren, (int)location.X, (int)location.Y, (int)(location.X + vector.X), (int)(location.Y + vector.Y));
            
        }

        private static void RenderLevel(IntPtr ren, Level level)
        {
            var blockSprite = Services.Sprites["block"];

            var tileCount = new Point((WindowWidth / Level.BlockSize) + 2, (WindowHeight / Level.BlockSize) + 2);
            var tileStart = new Point(viewOffset.X / Level.BlockSize, viewOffset.Y / Level.BlockSize);
            var tileEnd = tileStart + tileCount;
            var drawStart = new Point(-(viewOffset.X % Level.BlockSize), -(viewOffset.Y % Level.BlockSize));
            var drawLocation = drawStart;

            for (int x = tileStart.X; x < tileEnd.X; x++)
            {
                drawLocation.Y = drawStart.Y;
                for (int y = tileStart.Y; y < tileEnd.Y; y++)
                {
                    var blockType = level.GetBlock(x, y);
                    if (blockType == BlockType.Block)
                        blockSprite.Draw(ren, drawLocation);

                    drawLocation.Y += Level.BlockSize;
                }
                drawLocation.X += Level.BlockSize;
            }
        }
    }
}
