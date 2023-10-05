using Amazon.Runtime.Internal.Util;
using TinyUrlApi.DataModels;
using TinyUrlApi.Models;
using TinyUrlApi.Models.Data;

namespace TinyUrlApi.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IShortUrlGenerator _shortUrlGenerator;
        private readonly IMemoryCache _cache;

        public UrlShortenerService(IUrlRepository urlRepository,
            IShortUrlGenerator shortUrlGenerator,
            IMemoryCache cache)
        {
            _urlRepository = urlRepository;
            _shortUrlGenerator = shortUrlGenerator;
            _cache = cache;
        }

        public async Task<ShortenUrlResponse> GenerateShortUrl(ShortenUrlRequest request)
        {
            var urlEntry = await _urlRepository.GetUrlByFullUrl(request.Url);
            if(urlEntry != null)
            {
                return new ShortenUrlResponse()
                {
                    ShortUrl = urlEntry.ShortUrl
                };
            }

            var shortenedUrl = _shortUrlGenerator.GenerateShortUrl(request.Url);

            await _urlRepository.AddUrl(new ShortenedUrlEntry()
            {
                FullUrl = request.Url,
                ShortUrl = shortenedUrl
            });

            return new ShortenUrlResponse()
            {
                ShortUrl = shortenedUrl
            };
        }

        public async Task<RedirectToUrlResponse> RedirectToUrl(RedirectToUrlRequest request)
        {
            var fullUrl = _cache.TryGet(request.ShortUrl);
            if(fullUrl != null)
            {
                return new RedirectToUrlResponse()
                {
                    FullUrl = fullUrl
                };
            }

            var urlEntry = await _urlRepository.GetUrlByShortUrl(request.ShortUrl);

            if(urlEntry == null)
            {
                throw new NullReferenceException("Short url doesn't exists in database or expired");
            }

            _cache.Add(urlEntry);

            return new RedirectToUrlResponse()
            {
                FullUrl = urlEntry.FullUrl
            };
        }
    }
}
