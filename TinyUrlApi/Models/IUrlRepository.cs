using TinyUrlApi.DataModels;

namespace TinyUrlApi.Models
{
    public interface IUrlRepository
    {
        Task AddUrl(ShortenedUrlEntry shortenedUrl);
        Task DeleteUrl(ShortenedUrlEntry shortenedUrl);
        Task<ShortenedUrlEntry> GetUrlByFullUrl(string fullUrl);
        Task<ShortenedUrlEntry> GetUrlByShortUrl(string shortUrl);
    }
}
