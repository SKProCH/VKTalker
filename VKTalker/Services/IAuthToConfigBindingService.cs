using System.Threading.Tasks;

namespace VKTalker.Services {
    public interface IAuthToConfigBindingService {
        public Task<bool> TryLoginFromConfigAsync();
    }
}