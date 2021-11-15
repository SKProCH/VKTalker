using System.Threading.Tasks;

namespace VKTalker.Services {
    public interface IConfig {
        public Task Save();
        public string? AccessToken { get; set; }
    }
}