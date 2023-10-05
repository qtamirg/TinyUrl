using TinyUrlApi.DataModels;

namespace TinyUrlApi.Models
{
    public interface IMemoryCache
    {
        string? TryGet(string shortUrl);
        void Add(ShortenedUrlEntry urlEntry);
    }
}
