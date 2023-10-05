using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyUrlApi.Models;
using TinyUrlApi.Models.Data;

namespace TinyUrlApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerContoller : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerContoller(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost("ShortenUrl")]
        public async Task<IActionResult> ShortenUrl(ShortenUrlRequest request)
        {
            var generateShortUrlResponse = await _urlShortenerService.GenerateShortUrl(request);
            return Ok(generateShortUrlResponse);
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectToUrl(string shortUrl)
        {
            var request = new RedirectToUrlRequest() { ShortUrl = shortUrl };
            var redirectResponse = await _urlShortenerService.RedirectToUrl(request);
            return Redirect(redirectResponse.FullUrl);
        }
    }
}
