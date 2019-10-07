using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FileFormats.Levels
{
    public static class Loader
    {
        public static LevelFile Load(string file)
        {
            var jsonLevel = File.ReadAllText(file, Encoding.UTF8);

            return JsonSerializer.Deserialize<LevelFile>(jsonLevel);
        }
    }
}
