using TinyUrlApi.Models.Data;

namespace TinyUrlApi.Models
{
    public interface IUrlShortenerService
    {
        Task<ShortenUrlResponse> GenerateShortUrl(ShortenUrlRequest request);
        Task<RedirectToUrlResponse> RedirectToUrl(RedirectToUrlRequest request);
    }
}
