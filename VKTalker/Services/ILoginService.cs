using System;
using System.Threading.Tasks;

namespace VKTalker.Services {
    public interface ILoginService {
        IObservable<bool> ClientStateChanged { get; }
        bool IsAuthorized { get; }
        Task LoginAsync(string accessToken);
        Task LoginAsync(string login, string password);
        Task LogoutAsync();
    }
}