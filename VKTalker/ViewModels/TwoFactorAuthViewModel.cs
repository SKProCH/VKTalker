using ReactiveUI.Fody.Helpers;

namespace VKTalker.ViewModels {
    public class TwoFactorAuthViewModel : ViewModelBase {
        [Reactive] public string Code { get; set; }
    }
}