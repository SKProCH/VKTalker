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
            ActiveViewModel = GetViewModel(loginService.IsAuthorized);
            this.WhenActivated(disposable => {
                _loginService.ClientChanged
                    .Select(l => l == null)
                    .Select(GetViewModel)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .BindTo(this, model => model.ActiveViewModel)
                    .DisposeWith(disposable);
            });
        }
        private ViewModelBase GetViewModel(bool isAuthorized) {
            if (isAuthorized)
                return ActiveViewModel as MainViewModel ?? Locator.Current.GetService<MainViewModel>()!;
            else
                return ActiveViewModel as LoginViewModel ?? Locator.Current.GetService<LoginViewModel>()!;
        }

        [Reactive] public ViewModelBase ActiveViewModel { get; set; }
    }
}