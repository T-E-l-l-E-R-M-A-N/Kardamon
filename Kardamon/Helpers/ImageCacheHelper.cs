using System.Net.Http;
using System.Threading;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.Caching.Memory;

namespace Kardamon.Helpers;

public static class ImageCacheHelper
{
    private static readonly HttpClient _httpClient = new HttpClient();

    // Ограничение по размеру кеша, например 200 МБ
    private static readonly MemoryCache _cache = new(new MemoryCacheOptions
    {
        SizeLimit = 1024L * 1024L * 200L
    });

    public static async Task<Bitmap?> LoadFromWebAsync(
        Uri url,
        int targetWidth,
        CancellationToken cancellationToken = default)
    {
        var key = $"{url}|{targetWidth}";

        if (_cache.TryGetValue(key, out Bitmap cached))
            return cached;

        var bytes = await _httpClient.GetByteArrayAsync(url, cancellationToken);

        await using var ms = new MemoryStream(bytes);

        // ВАЖНО: декодируем сразу в нужную ширину
        var bitmap = Bitmap.DecodeToWidth(ms, targetWidth);

        _cache.Set(
            key,
            bitmap,
            new MemoryCacheEntryOptions
            {
                Size = bytes.Length,                    // вес записи
                SlidingExpiration = TimeSpan.FromMinutes(10)
            });

        return bitmap;
    }
}