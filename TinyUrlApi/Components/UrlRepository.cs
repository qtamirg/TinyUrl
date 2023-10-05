using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using TinyUrlApi.DataModels;
using TinyUrlApi.Models;

namespace TinyUrlApi.Components
{
    public class UrlRepository : IUrlRepository
    {
        private readonly IOptions<ShortenedUrlConnectionParameters> _databaseConnectionParameters;
        private readonly IMongoCollection<ShortenedUrlEntry> _shortenedUrlRepository;

        public UrlRepository(IOptions<ShortenedUrlConnectionParameters> databaseConnectionParameters)
        {
            _databaseConnectionParameters = databaseConnectionParameters;

            var connectionParameters = _databaseConnectionParameters.Value;

            IMongoClient client = new MongoClient(connectionParameters.ConnectionUrl);
            IMongoDatabase database = client.GetDatabase(connectionParameters.DatabaseName);
            _shortenedUrlRepository = database.GetCollection<ShortenedUrlEntry>(connectionParameters.CollectionName);
        }

        public async Task AddUrl(ShortenedUrlEntry shortenedUrl)
        {
            await _shortenedUrlRepository.InsertOneAsync(shortenedUrl);
        }

        public async Task DeleteUrl(ShortenedUrlEntry shortenedUrl)
        {
            await _shortenedUrlRepository.DeleteOneAsync(urlEntry => urlEntry.FullUrl == shortenedUrl.FullUrl);
        }

        public async Task<ShortenedUrlEntry> GetUrlByFullUrl(string fullUrl)
        {
            IAsyncCursor<ShortenedUrlEntry> cursor = await _shortenedUrlRepository.FindAsync(url => url.FullUrl == fullUrl);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<ShortenedUrlEntry> GetUrlByShortUrl(string shortUrl)
        {
            IAsyncCursor<ShortenedUrlEntry> cursor = await _shortenedUrlRepository.FindAsync(url => url.ShortUrl == shortUrl);
            return await cursor.FirstOrDefaultAsync();
        }
    }
}
