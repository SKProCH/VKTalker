using System.IO;
using System.Text.Json;


namespace VKTalker.Models
{
    public class ConfigModel
    {
        public ulong AppId { get; set; }
        public string? AccessToken { get; set; }

        public static ConfigModel CreateConfig(string filename)
        {
            var text = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<ConfigModel>(text);
        }
    }
}