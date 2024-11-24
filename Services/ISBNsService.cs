using Microsoft.Extensions.Caching.Distributed;
using RASP_Redis.Models;

namespace RASP_Redis.Services
{
    public class ISBNsService
    {
        private readonly IDistributedCache _cache;

        public ISBNsService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string?> GetCachedDocIdAsync(string isbn)
        {
            try
            {
                return await _cache.GetStringAsync(isbn);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing cache for ISBN {isbn}: {ex.Message}");
                return null;
            }
        }

        // Cache a single ISBN with its docId
        public async Task CacheISBNAsync(string isbn, string docId)
        {
            if (string.IsNullOrEmpty(isbn) || string.IsNullOrEmpty(docId))
            {
                throw new ArgumentException("ISBN and docId must not be null or empty.");
            }

            var cacheOptions = new DistributedCacheEntryOptions();


            // Store docId with ISBN as key
            await _cache.SetStringAsync(isbn, docId, cacheOptions);
        }

        public async Task RemovedCachedISBNAsync(string isbn)
        {
            await _cache.RemoveAsync(isbn);
        }
    }

}
