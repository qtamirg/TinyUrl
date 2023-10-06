using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;
using TinyUrlApi.DataModels;
using TinyUrlApi.Models;

namespace TinyUrlApi.Components
{
    public class RedirectionsMemoryCache : IMemoryCache
    {
        // Cache size should be configurable from a Database or be coming from the AppSettings level and injected. 
        // Only setting as constant to save time.
        private const int CACHE_SIZE = 1024;
        private const int CONCURRENCY_LEVEL = 5;

        private readonly ConcurrentDictionary<string, string> _cachedUrls;
        private readonly ConcurrentQueue<string> _leastRecentlyUsedQueue;
        private readonly Semaphore _semaphoreObject;

        public RedirectionsMemoryCache()
        {
            _cachedUrls = new ConcurrentDictionary<string, string>(CONCURRENCY_LEVEL, CACHE_SIZE);
            _leastRecentlyUsedQueue = new ConcurrentQueue<string>();
            _semaphoreObject = new Semaphore(CONCURRENCY_LEVEL, CONCURRENCY_LEVEL);
        }

        public void Add(ShortenedUrlEntry urlEntry)
        {
            try
            {
                _semaphoreObject.WaitOne();

                if (_cachedUrls.Count >= CACHE_SIZE)
                {
                    _leastRecentlyUsedQueue.TryDequeue(out string leastRecentlyUsedKey);
                    _cachedUrls.Remove(leastRecentlyUsedKey, out string fullUrl);
                }

                _cachedUrls[urlEntry.ShortUrl] = urlEntry.FullUrl;
                _leastRecentlyUsedQueue.Enqueue(urlEntry.ShortUrl);
            }
            finally
            {
                _semaphoreObject.Release();
            }
        }

        public string? TryGet(string shortUrl)
        {
            try
            {
                _semaphoreObject.WaitOne();

                string? fullUrl = null;
                if (_cachedUrls.TryGetValue(shortUrl, out fullUrl))
                {
                    _leastRecentlyUsedQueue.Enqueue(shortUrl);
                    _leastRecentlyUsedQueue.TryDequeue(out string shortUrlToEvict);
                }

                return fullUrl;
            }
            finally
            {
                _semaphoreObject.Release();
            }
        }
    }
}
