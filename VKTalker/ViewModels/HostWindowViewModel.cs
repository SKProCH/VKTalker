using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using VKTalker.Services;

namespace VKTalker.ViewModels
{
    public class HostWindowViewModel : ViewModelBase
    {
        private ILoginService _loginService;
        public HostWindowViewModel(ILoginService loginService) {
            _loginService = loginService;
            _loginService.ClientStateChanged
                .Select(b => (ViewModelBase)(b ? Locator.Current.GetService<MainViewModel>() : Locator.Current.GetService<LoginViewModel>())!)
                .BindTo(this, model => model.ActiveViewModel);
        }
        
        [Reactive] public ViewModelBase ActiveViewModel { get; set; } 
    }
}