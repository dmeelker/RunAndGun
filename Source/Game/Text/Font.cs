using SDL2;
using Game.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Game.Text
{
    public class Font
    {
        private class Character
        {
            public int Id;
            public int X;
            public int Y;
            public int Width;
            public int Height;
            public int Xoffset;
            public int Yoffset;
            public int Xadvance;
            public Sprite Image;
        }

        public int LineHeight { get; private set; }
        private int _base;

        private readonly IntPtr textureId;
        private Dictionary<int, Character> _characterMap = new Dictionary<int, Character>();
        private List<Character> _characters = new List<Character>();
        private Dictionary<Tuple<int, int>, int> _kerning = new Dictionary<Tuple<int, int>, int>();

        public Font(Stream stream, IntPtr textureId)
        {
            this.textureId = textureId;
            XmlSerializer serializer = new XmlSerializer(typeof(Model.Font));

            using (var reader = new StreamReader(stream))
            {
                var data = (Model.Font)serializer.Deserialize(reader);

                LineHeight = data.Common.LineHeight;
                _base = data.Common.Base;

                _characters = data.Characters.Select(chr => new Character
                {
                    Id = chr.Id,
                    X = chr.X,
                    Y = chr.Y,
                    Width = chr.Width,
                    Height = chr.Height,
                    Xoffset = chr.XOffset,
                    Yoffset = chr.YOffset,
                    Xadvance = chr.XAdvance,
                    Image = new Sprite(this.textureId, chr.X, chr.Y, chr.Width, chr.Height)
                }).ToList();
                _characterMap = _characters.ToDictionary(chr => chr.Id);

                if(data.Kernings != null)
                    _kerning = data.Kernings.ToDictionary(kerning => Tuple.Create(kerning.First, kerning.Second), kerning => kerning.Amount);
            }
        }

        public void Render(IntPtr rendererId, string text, int x, int y)
        {
            //SDL.SDL_SetTextureColorMod(textureId, 255, 0, 0);
            for (int i = 0; i < text.Length; i++)
            {
                var characterCode = (int)text[i];

                if (!_characterMap.TryGetValue(characterCode, out var character))
                    continue;

                character.Image.Draw(rendererId, x + character.Xoffset, y + character.Yoffset);
                x += character.Xadvance;

                if (i < text.Length - 1)
                {
                    int nextCharacterCode = text[i + 1];

                    if (_kerning.TryGetValue(Tuple.Create(characterCode, nextCharacterCode), out var kerning))
                        x += kerning;
                }
            }
        }

        public int GetStringWidth(string text)
        {
            var width = 0;
            for (int i = 0; i < text.Length; i++)
            {
                var characterCode = (int)text[i];

                if (!_characterMap.TryGetValue(characterCode, out var character))
                    continue;

                width += character.Xadvance;

                if (i < text.Length - 1)
                {
                    int nextCharacterCode = text[i + 1];

                    if (_kerning.TryGetValue(Tuple.Create(characterCode, nextCharacterCode), out var kerning))
                        width += kerning;
                }
            }

            return width;
        }
    }
}
