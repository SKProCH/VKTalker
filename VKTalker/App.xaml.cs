using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Splat;
using VkNet;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;
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
            // Registering services
            SplatRegistrations.SetupIOC();
            SplatRegistrations.RegisterLazySingleton<IImageLoader, DiskAndMemoryImageLoader>();
            Locator.CurrentMutable.RegisterConstant(ConfigModel.CreateConfig("Config.json"), typeof(ConfigModel));
            var vkApi = new VkApi(new ServiceCollection().AddAudioBypass());
            Locator.CurrentMutable.RegisterConstant(vkApi);
            Locator.CurrentMutable.RegisterConstant(vkApi, typeof(IVkApiAuth));
            SplatRegistrations.RegisterLazySingleton<ILoginService, VkLoginService>();
            
            // Register view models
            SplatRegistrations.Register<HostWindowViewModel, HostWindowViewModel>();
            SplatRegistrations.Register<LoginViewModel, LoginViewModel>();
            SplatRegistrations.Register<MainViewModel, MainViewModel>();
            SplatRegistrations.Register<TwoFactorAuthViewModel, TwoFactorAuthViewModel>();
            
            // Trying perform auth
            // TODO: Wrap it into service
            var accessToken = Locator.Current.GetService<ConfigModel>()?.AccessToken;
            if (accessToken != null) Locator.Current.GetService<ILoginService>()?.LoginAsync(accessToken);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new HostWindow
                {
                    DataContext = Locator.Current.GetService<HostWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}