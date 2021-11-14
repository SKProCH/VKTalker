using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VKTalker.ViewModels;

namespace VKTalker.Views
{
    public class HostWindow : ReactiveWindow<HostWindowViewModel>
    {
        public HostWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            HostContentPresenter = this.FindControl<ContentPresenter>("HostContentPresenter");
            this.WhenActivated(disposable => {
                this.WhenAnyValue(window => window.ViewModel!.ActiveViewModel)
                    .BindTo(this, window => window.HostContentPresenter.Content)
                    .DisposeWith(disposable);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public ContentPresenter HostContentPresenter { get; }
    }
}