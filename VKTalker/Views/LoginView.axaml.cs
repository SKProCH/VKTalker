using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using VKTalker.ViewModels;

namespace VKTalker.Views {
    public class LoginView : ReactiveUserControl<LoginViewModel> {
        public LoginView() {
            InitializeComponent();
            PasswordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            LoginTextBox = this.FindControl<TextBox>("LoginTextBox");
            this.WhenActivated(disposable => {
                this.BindValidationToTextBoxErrors(ViewModel, model => model.Login, view => LoginTextBox, true)
                    .DisposeWith(disposable);
                this.BindValidationToTextBoxErrors(ViewModel, model => model.Password, view => PasswordTextBox, true)
                    .DisposeWith(disposable);
            });
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }

        private TextBox PasswordTextBox { get; }
        private TextBox LoginTextBox { get; }
    }
}