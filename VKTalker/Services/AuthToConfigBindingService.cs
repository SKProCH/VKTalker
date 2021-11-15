using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VKTalker.Models;

namespace VKTalker.Services {
    class AuthToConfigBindingService : IAuthToConfigBindingService {
        private readonly ILoginService _loginService;
        private readonly ConfigModel _configModel;
        public AuthToConfigBindingService(ILoginService loginService, ConfigModel configModel) {
            _loginService = loginService;
            _configModel = configModel;
            _loginService.ClientStateChanged
                .Select(b => _loginService.AccessToken)
                .Subscribe(SaveAccessTokenToConfig);
        }

        private void SaveAccessTokenToConfig(string? token) {
            _configModel.AccessToken = token;
            _configModel.Save();
        }

        public async Task<bool> TryLoginFromConfigAsync() {
            try {
                if (_configModel.AccessToken != null) {
                    await _loginService.LoginAsync(_configModel.AccessToken);
                }
            }
            catch (Exception) {
                // ignored
            }
            return _loginService.IsAuthorized;
        }
    }
}