using System.IO;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace VKTalker.Models
{
    public class ConfigModel
    {
        public string? AccessToken { get; set; }

        public static ConfigModel CreateConfig(string filename)
        {
            var text = File.ReadAllText(filename);
            return JsonSerializer.Deserialize<ConfigModel>(text);
        }

        public void Save() {
            // TODO: Refactor ConfigModel
            File.WriteAllText("Config.json", JsonConvert.SerializeObject(this));
        }
    }
}