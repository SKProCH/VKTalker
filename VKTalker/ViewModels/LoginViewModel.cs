using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using VKTalker.Services;

namespace VKTalker.ViewModels {
    public class LoginViewModel : ViewModelBase {
        private ILoginService _loginService;
        public LoginViewModel(ILoginService loginService) {
            _loginService = loginService;
            LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync, this.IsValid());
            ForgetPassword = ReactiveCommand.Create(OpenForgetPasswordPage);

            this.ValidationRule(model => model.Login,
                s => !string.IsNullOrWhiteSpace(s),
                "Логин не может быть пустой строкой");
            this.ValidationRule(model => model.Password,
                s => !string.IsNullOrWhiteSpace(s),
                "Пароль не может быть пустой строкой");
        }

        private void OpenForgetPasswordPage() {
            Process.Start("https://id.vk.com/restore/");
        }

        private Task LoginAsync() {
            return _loginService.LoginAsync(Login, Password);
        }

        [Reactive] public string Login { get; set; } = "";
        [Reactive] public string Password { get; set; } = "";
        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> ForgetPassword { get; }
    }
}