using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using VKTalker.ViewModels;

namespace VKTalker.Views {
    public class TwoFactorAuthView : ReactiveUserControl<LoginViewModel> {
        public TwoFactorAuthView() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            AvaloniaXamlLoader.Load(this);
        }
    }
}