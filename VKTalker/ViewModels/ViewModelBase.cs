using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace VKTalker.ViewModels {
    public class ViewModelBase : ReactiveObject, IValidatableViewModel {
        public ValidationContext ValidationContext { get; }
    }
}