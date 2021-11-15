using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKTalker.Services {
    public class FileConfig : IConfig {
        private ConfigModel _model;
        private string _path;
        
        public static FileConfig LoadOrCreate(string path) {
            return new FileConfig(path);
        }

        private FileConfig(string path) {
            _path = path;
            try {
                var configText = File.ReadAllText(path);
                _model = JsonConvert.DeserializeObject<ConfigModel>(configText) ?? new ConfigModel();
            }
            catch (Exception) {
                _model = new ConfigModel();
            }
        }
        
        public Task Save() {
            return File.WriteAllTextAsync(_path, JsonConvert.SerializeObject(_model));
        }
        
        public string? AccessToken {
            get => _model.AccessToken;
            set => _model.AccessToken = value;
        }

        private class ConfigModel
        {
            public string? AccessToken { get; set; }
        }
    }
}