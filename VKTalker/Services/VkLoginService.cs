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
        private BehaviorSubject<bool> _authorizationInProgressChanged = new(false);
        private VkApi _vkApiAuth;
        public VkLoginService(VkApi vkApiAuth) {
            _vkApiAuth = vkApiAuth;
        }

        public IObservable<bool> ClientStateChanged => _clientStateChanged.AsObservable();
        public bool IsAuthorized => _vkApiAuth.IsAuthorized;
        public string? AccessToken => _vkApiAuth.Token;
        public IObservable<bool> AuthorizationInProgressChanged => _authorizationInProgressChanged.AsObservable();
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

        private async Task LoginAsyncInternal(IApiAuthParams apiAuthParams) {
            try {
                _authorizationInProgressChanged.OnNext(true);
                await _vkApiAuth.LogOutAsync();
                _clientStateChanged.OnNext(IsAuthorized);
                await _vkApiAuth.AuthorizeAsync(apiAuthParams);
                _clientStateChanged.OnNext(IsAuthorized);
            }
            finally {
                _authorizationInProgressChanged.OnNext(false);
            }
        }

        public Task LogoutAsync() {
            _vkApiAuth.LogOut();
            _clientStateChanged.OnNext(IsAuthorized);
            return Task.CompletedTask;
        }
    }
}