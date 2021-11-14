using System.Reactive.Linq;
using Avalonia.Controls.Mixins;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using VKTalker.Services;

namespace VKTalker.ViewModels
{
    public class HostWindowViewModel : ViewModelBase, IActivatableViewModel {
        public ViewModelActivator Activator { get; } = new ViewModelActivator();
        
        private readonly ILoginService _loginService;
        public HostWindowViewModel(ILoginService loginService) {
            _loginService = loginService;
            ActiveViewModel = Locator.Current.GetService<LoginViewModel>()!;
            this.WhenActivated(disposable => {
                _loginService.ClientStateChanged
                    .Select(b => (ViewModelBase)(b ? Locator.Current.GetService<MainViewModel>() : Locator.Current.GetService<LoginViewModel>())!)
                    .BindTo(this, model => model.ActiveViewModel)
                    .DisposeWith(disposable);
            });
        }
        
        [Reactive] public ViewModelBase ActiveViewModel { get; set; }
    }
}