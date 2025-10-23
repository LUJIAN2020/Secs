using Secs.Demo.Models;
using System;
using System.IO;
using System.Text.Json;

namespace Secs.Demo.Commons
{
    public class ConfigHelper
    {
        private static readonly string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        public static void SaveConfig(ConfigParameter config)
        {
            string str = JsonSerializer.Serialize(config, typeof(ConfigParameter), MyGenerationContext.Default);
            File.WriteAllText(fileName, str);
        }

        public static ConfigParameter LoadConfig()
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    var config = new ConfigParameter();
                    SaveConfig(config);
                    return config;
                }
                string str = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize(str, MyGenerationContext.Default.ConfigParameter) ?? new();
            }
            catch
            {
                return new();
            }
        }
    }
}
