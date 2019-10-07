using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace SdlTest
{
    public class TextureManager
    {
        private readonly Dictionary<string, IntPtr> textures = new Dictionary<string, IntPtr>();

        public void LoadTexture(IntPtr renderer, string filename, string key)
        {
            var texture = SDL_image.IMG_LoadTexture(renderer, filename);
            textures.Add(key, texture);
        }

        public IntPtr this[string key] => textures[key];

        public void Cleanup()
        {
            foreach(var texture in textures.Values)
            {
                SDL.SDL_DestroyTexture(texture);
            }

            textures.Clear();
        }
    }
}
