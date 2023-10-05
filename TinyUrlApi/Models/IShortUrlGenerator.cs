namespace TinyUrlApi.Models
{
    public interface IShortUrlGenerator
    {
        string GenerateShortUrl(string url);
    }
}
