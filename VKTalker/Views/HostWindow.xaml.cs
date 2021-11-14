using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}