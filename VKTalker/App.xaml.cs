using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Splat;
using VKTalker.Models;
using VKTalker.Services;
using VKTalker.ViewModels;
using VKTalker.Views;

namespace VKTalker
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Locator.CurrentMutable.RegisterConstant(new DiskAndMemoryImageLoader(), typeof(IImageLoader));
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(ConfigModel.CreateConfig("Config.json")),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}