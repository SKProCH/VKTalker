using System;
using System.Threading.Tasks;

namespace VKTalker.Services {
    public interface ILoginService {
        IObservable<bool> ClientStateChanged { get; }
        bool IsAuthorized { get; }
        string? AccessToken { get; }
        Task LoginAsync(string accessToken);
        Task LoginAsync(string login, string password, Func<string>? twoFactorAuthorization = null);
        Task LogoutAsync();
    }
}