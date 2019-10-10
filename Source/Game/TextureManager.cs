using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class TextureManager
    {
        private readonly Dictionary<string, IntPtr> textures = new Dictionary<string, IntPtr>();

        public IntPtr LoadTexture(IntPtr renderer, string filename, string key)
        {
            var texture = SDL_image.IMG_LoadTexture(renderer, filename);
            textures.Add(key, texture);
            
            return texture;
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
