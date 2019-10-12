using SDL2;
using Game.Text;
using System;
using System.IO;

namespace Game
{
    class Program
    {
        static IntPtr win;
        static IntPtr ren;

        public static bool quit = false;

        public const int WindowWidth = 800;
        public const int WindowHeight = 600;

        static IntPtr cursorSurface;
        static IntPtr cursor;

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

            cursorSurface = SDL_image.IMG_Load("Resources/Cursors/crosshair.png");
            cursor = SDL.SDL_CreateColorCursor(cursorSurface, 18, 18);
            SDL.SDL_SetCursor(cursor);

            LoadTextures();

            Services.Game.LoadLevel(FileFormats.Levels.Loader.Load(@"Resources\Levels\level1.json"));

            uint lastUpdateTime = SDL.SDL_GetTicks();

            while (!quit)
            {
                var time = SDL.SDL_GetTicks();
                Services.Time = time;
                var timePassed = (int)(time - lastUpdateTime);
                lastUpdateTime = time;

                Services.Game.Update(time, timePassed);
                Services.Game.Render(ren, time);
            }

            Services.Textures.Cleanup();
            SDL.SDL_FreeCursor(cursor);
            SDL.SDL_FreeSurface(cursorSurface);
            SDL.SDL_DestroyRenderer(ren);
            SDL.SDL_DestroyWindow(win);
            SDL.SDL_Quit();
        }
        
        private static void LoadTextures()
        {
            //Services.Textures.LoadTexture(ren, "Resources/test.png", "player");
            Services.Textures.LoadTexture(ren, "Resources/block.png", "block");
            Services.Textures.LoadTexture(ren, "Resources/projectile.png", "projectile");
            Services.Textures.LoadTexture(ren, "Resources/crate.png", "crate");
            
            Services.Textures.LoadTexture(ren, "Resources/Backdrops/city.png", "backdrop");

            LoadTextureAndSprite("Resources/Gibs/gib.png", "gib");
            LoadTextureAndSprite("Resources/Gibs/floor-blood.png", "floor-blood");

            LoadTextureAndSprite("Resources/Weapons/pistol.png", "pistol");
            LoadTextureAndSprite("Resources/Weapons/shotgun.png", "shotgun");
            LoadTextureAndSprite("Resources/Weapons/submachinegun.png", "submachinegun");
            LoadTextureAndSprite("Resources/Weapons/sniperrifle.png", "sniperrifle");

            LoadTextureAndSprite("Resources/Weapons/bulletcasing.png", "bulletcasing");
            LoadTextureAndSprite("Resources/Weapons/shotgunshell.png", "shotgunshell");

            LoadTextureAndSprite("Resources/Collectables/medpack.png", "medpack");
            LoadTextureAndSprite("Resources/Collectables/armor.png", "armor");

            LoadTextureAndSprite("Resources/UI/HealthAndArmorFrame.png", "healthAndArmorFrame");
            LoadTextureAndSprite("Resources/UI/armorBar.png", "armorBar");
            LoadTextureAndSprite("Resources/UI/healthBar.png", "healthBar");

            LoadTextureAndSprite("Resources/Characters/player.png", "player");

            //Services.Sprites.Add(new Sprites.Sprite(Services.Textures["player"]), "player");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["block"]), "block");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["projectile"]), "projectile");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["crate"]), "crate");
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures["backdrop"]), "backdrop");

            LoadFont("Resources/Font/DTM-Sans_0.png", "DTM-Sans.fnt", "default");
            LoadFont("Resources/Font/Sabo-Filled_0.png", "Sabo-Filled.fnt", "big");
        }

        private static void LoadTextureAndSprite(string fileName, string name)
        {
            Services.Textures.LoadTexture(ren, fileName, name);
            Services.Sprites.Add(new Sprites.Sprite(Services.Textures[name]), name);
        }

        private static void LoadFont(string imageFile, string fontDescriptorFile, string name)
        {
            var texture = Services.Textures.LoadTexture(ren, imageFile, "Font-" + name);
            using var fontFile = File.OpenRead(Path.Combine("Resources", "font", fontDescriptorFile));
            var font = new Font(fontFile, texture);
            Services.Fonts.Add(font, name);
        }

        public static void SetWindowTitle(string title)
        {
            SDL.SDL_SetWindowTitle(win, title);
        }
    }
}
