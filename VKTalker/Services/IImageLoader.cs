using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace VKTalker.Services {
    public interface IImageLoader {
        public Task<IBitmap?> LoadImageAsync(string url);
    }
}