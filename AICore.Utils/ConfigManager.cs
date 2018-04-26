using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace AICore.Utils
{
    public class ConfigManager
    {
        private static IConfigurationSection appSections = null;

        public static string AppSettings(string key)
        {
            string str = "";
            if (appSections.GetSection(key) != null)
            {
                str = appSections.GetSection(key).Value;
            }
            return str;
        }


        public static void SetAppSettings(IConfigurationSection section)
        {
            appSections = section;
        }
    }
}
