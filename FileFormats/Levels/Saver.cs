using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FileFormats.Levels
{
    public static class Saver
    {
        public static void Save(LevelFile level, string file)
        {
            var jsonLevel = JsonSerializer.Serialize<LevelFile>(level, new JsonSerializerOptions {
                WriteIndented = true 
            });

            File.WriteAllText(file, jsonLevel, Encoding.UTF8);
        }
    }
}
