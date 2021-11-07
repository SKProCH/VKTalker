using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Flurl.Http;

namespace VKTalker.Services {
    public class DiskAndMemoryImageLoader : IImageLoader {
        private readonly ConcurrentDictionary<string, Task<IBitmap?>> _memoryCache = new();
        public async Task<IBitmap?> LoadImageAsync(string url) {
            var bitmap = await _memoryCache.GetOrAdd(url, LoadImageInternalAsync);
            // If load failed - remove from cache and return
            // Next load attempt will try to load image again
            if (bitmap == null) _memoryCache.TryRemove(url, out _);
            return bitmap;
        }
        
        private async Task<IBitmap?> LoadImageInternalAsync(string url) {
            var hash = Utilities.CreateMD5(url);

            var fileName = $"Cache/Images/{hash}";
            if (File.Exists(fileName)) {
                return new Bitmap(fileName);
            }

            try {
                var imageBytes = await url.GetBytesAsync();
                Directory.CreateDirectory("Cache/Images");
                await File.WriteAllBytesAsync(fileName, imageBytes);
                return new Bitmap(fileName);
            }
            catch (Exception) {
                return null;
            }
        }
    }
}