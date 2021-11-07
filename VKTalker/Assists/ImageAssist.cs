using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Splat;
using VKTalker.Services;

namespace VKTalker.Assists {
    public static class ImageAssist {
        static ImageAssist() {
            var imageLoaderService = Locator.Current.GetService<IImageLoader>();
            SourceUrlProperty.Changed.Subscribe(async args => {
                var bitmap = await imageLoaderService.LoadImageAsync(args.NewValue.Value);
                if (GetSourceUrl((args.Sender as Image)!) != args.NewValue.Value) return;
                Dispatcher.UIThread.Post(() => {
                    (args.Sender as Image)!.Source = bitmap;
                });
            });
        }

        public static readonly AttachedProperty<string> SourceUrlProperty = AvaloniaProperty.RegisterAttached<Image, string>("SourceUrl", typeof(ImageAssist));

        public static string GetSourceUrl(Image element) {
            return element.GetValue(SourceUrlProperty);
        }

        public static void SetSourceUrl(Image element, string value) {
            element.SetValue(SourceUrlProperty, value);
        }
    }
}