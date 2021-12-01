using System;
using System.Threading.Tasks;

namespace VKTalker.Services {
    public interface ILoginService {
        IObservable<long?> ClientChanged { get; }
        bool IsAuthorized { get; }
        string? AccessToken { get; }
        IObservable<bool> AuthorizationInProgressChanged { get; }
        long? CurrentUserId { get; }
        Task LoginAsync(string accessToken);
        Task LoginAsync(string login, string password, Func<string>? twoFactorAuthorization = null);
        Task LogoutAsync();
    }
}