using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using UpravlenieRC.Client;
using UpravlenieRC.Client.ViewModels;
using System.Collections.ObjectModel;
namespace UpravlenieRC.Client.modules
{
    static class Cache
    {
        private class CacherSerializer()
        {
            [JsonInclude]
            public Theme theme = AppSettings.GetTheme();
        }
        const string file = "cache.json";
        static string param;
        static CacherSerializer? cacher = new CacherSerializer();
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public static async void SaveCache()
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                cacher.theme = AppSettings.GetTheme();
                JsonSerializer.Serialize<CacherSerializer>(fs, cacher);
            }
        }
        public static async void LoadCache()
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                cacher = await JsonSerializer.DeserializeAsync<CacherSerializer>(fs);
            }
            AppSettings.LoadTheme(cacher.theme);

        }
    }
}
