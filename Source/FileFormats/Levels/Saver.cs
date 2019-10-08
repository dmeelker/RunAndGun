using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileFormats.Levels
{
    public static class Saver
    {
        public static void Save(LevelFile level, string file)
        {
            var jsonLevel = JsonConvert.SerializeObject(level, Formatting.Indented);

            File.WriteAllText(file, jsonLevel, Encoding.UTF8);
        }
    }
}
