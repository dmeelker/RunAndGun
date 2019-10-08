using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileFormats.Levels
{
    public static class Loader
    {
        public static LevelFile Load(string file)
        {
            var jsonLevel = File.ReadAllText(file, Encoding.UTF8);

            return JsonConvert.DeserializeObject<LevelFile>(jsonLevel);
        }
    }
}
