using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VKTalker.Services {
    public class VkLoginService : ILoginService {
        private Subject<bool> _clientStateChanged = new();
        private VkApi _vkApiAuth;
        public VkLoginService(VkApi vkApiAuth) {
            _vkApiAuth = vkApiAuth;
        }

        public IObservable<bool> ClientStateChanged => _clientStateChanged.AsObservable();
        public bool IsAuthorized => _vkApiAuth.IsAuthorized;
        public string? AccessToken => _vkApiAuth.Token;
        public Task LoginAsync(string accessToken) {
            return LoginAsyncInternal(new ApiAuthParams() { AccessToken = accessToken });
        }
        public Task LoginAsync(string login, string password, Func<string>? twoFactorAuthorization = null) {
            var apiAuthParams = new ApiAuthParams() {
                Login = login, Password = password, Settings = Settings.All,
                TwoFactorAuthorization = twoFactorAuthorization, TwoFactorSupported = twoFactorAuthorization != null
            };
            return LoginAsyncInternal(apiAuthParams);
        }

        private Task LoginAsyncInternal(ApiAuthParams apiAuthParams) {
            return Task.Run(() => {
                _vkApiAuth.LogOut();
                _clientStateChanged.OnNext(IsAuthorized);
                _vkApiAuth.Authorize(apiAuthParams);
                _clientStateChanged.OnNext(IsAuthorized);
            });
        }

        public Task LogoutAsync() {
            _vkApiAuth.LogOut();
            _clientStateChanged.OnNext(IsAuthorized);
            return Task.CompletedTask;
        }
    }
}