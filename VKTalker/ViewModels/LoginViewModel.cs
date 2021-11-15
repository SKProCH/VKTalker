using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using Splat;
using VKTalker.Services;

namespace VKTalker.ViewModels {
    public class LoginViewModel : ViewModelBase {
        private ILoginService _loginService;
        public LoginViewModel(ILoginService loginService) {
            _loginService = loginService;
            var canExecuteLogin = Observable.CombineLatest(this.IsValid(), this.WhenAnyValue(model => model.IsLoading), (b, b1) => b && !b1);
            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync, canExecuteLogin);

            this.ValidationRule(model => model.Login,
                s => !string.IsNullOrWhiteSpace(s),
                "Логин не может быть пустой строкой");
            this.ValidationRule(model => model.Password,
                s => !string.IsNullOrWhiteSpace(s),
                "Пароль не может быть пустой строкой");

            this.WhenAnyValue(model => model.IsLoading, model => model.ErrorText)
                .Select(tuple => tuple.Item1 || !string.IsNullOrEmpty(tuple.Item2))
                .BindTo(this, model => model.IsWarningShown);
        }

        private void OpenForgetPasswordPage() {
            Utilities.OpenUrlInBrowser("https://id.vk.com/restore/");
        }

        private async Task LoginAsync() {
            try {
                IsLoading = true;
                await _loginService.LoginAsync(Login, Password, GetTwoFactorAuthorizationCode);
            }
            catch (Exception e) {
                ErrorText = e.Message ?? $"Что-то пошло не так: {e.GetType().Name}";
            }
            finally {
                IsLoading = false;
            }
        }

        private string GetTwoFactorAuthorizationCode() {
            var taskCompletionSource = new TaskCompletionSource<object?>();
            Dispatcher.UIThread.Post(async () => {
                taskCompletionSource.SetResult(await DialogHost.DialogHost.Show(Locator.Current.GetService<TwoFactorAuthViewModel>()!, "Main"));
            });

            var result = taskCompletionSource.Task.GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(result as string)) {
                throw new Exception("Ввод кода двухфакторной авторизации отменен");
            }
            return (result as string)!;
        }

        [Reactive] public string Login { get; set; } = null!;
        [Reactive] public string Password { get; set; } = null!;
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        [Reactive] public bool IsWarningShown { get; set; }
        [Reactive] public bool IsLoading { get; set; }
        [Reactive] public string ErrorText { get; set; }
    }
}