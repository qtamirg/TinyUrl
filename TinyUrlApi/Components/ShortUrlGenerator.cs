using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using TinyUrlApi.Models;

namespace TinyUrlApi.Components
{
    public class ShortUrlGenerator : IShortUrlGenerator
    {
        private const int URL_LENGTH = 8;
        private const string ALLOWED_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


        public string GenerateShortUrl(string url)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(url));
                ulong hash = BitConverter.ToUInt64(hashBytes, 0);

                StringBuilder shortenedUrl = new StringBuilder(URL_LENGTH);
                for (int i = 0; i < URL_LENGTH; i++)
                {
                    int index = (int)(hash % (ulong)ALLOWED_CHARACTERS.Length);
                    shortenedUrl.Append(ALLOWED_CHARACTERS[index]);
                    hash >>= URL_LENGTH;
                }

                return shortenedUrl.ToString();
            }
        }
    }
}
