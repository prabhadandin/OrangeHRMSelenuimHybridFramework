using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public static class ConfigReader
    {
        private static readonly Dictionary<string, string> config;

        static ConfigReader()
        {
            var json = File.ReadAllText("appsettings.json");
            config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        public static string Get(string key)
        {
            return config.ContainsKey(key) ? config[key] : string.Empty;
        }
    }
}
