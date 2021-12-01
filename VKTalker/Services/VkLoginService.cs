using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using VkNet;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VKTalker.Services {
    public class VkLoginService : ILoginService {
        private Subject<long?> _clientChanged = new();
        private BehaviorSubject<bool> _authorizationInProgressChanged = new(false);
        private VkApi _vkApiAuth;
        public VkLoginService(VkApi vkApiAuth) {
            _vkApiAuth = vkApiAuth;
        }

        public IObservable<long?> ClientChanged => _clientChanged.AsObservable();
        public bool IsAuthorized => _vkApiAuth.IsAuthorized;
        public string? AccessToken => _vkApiAuth.Token;
        public long? CurrentUserId { get; private set; }
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
                await LogoutAsync();
                await _vkApiAuth.AuthorizeAsync(apiAuthParams);
                CurrentUserId = await _vkApiAuth.Users.GetAsync(Array.Empty<long>()).PipeAsync(users => users.First().Id);
                _clientChanged.OnNext(CurrentUserId);
            }
            finally {
                _authorizationInProgressChanged.OnNext(false);
            }
        }

        public async Task LogoutAsync() {
            await _vkApiAuth.LogOutAsync();
            CurrentUserId = null;
            _clientChanged.OnNext(CurrentUserId);
        }
    }
}