using StackExchange.Redis;

namespace RASP_Redis.Services.Redis
{
    public class BookStoreRedisService
    {
        private readonly IDatabase _redisDb;

        public BookStoreRedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase();
        }
        public async Task<string?> GetCachedDocIdAsync(string isbn)
        {
            try
            {
                var result = await _redisDb.StringGetAsync(isbn); 
                return result.IsNullOrEmpty ? null : result.ToString();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error accessing cache for ISBN {isbn}: {ex.Message}");
                return null;
            }
        }

        public async Task CacheISBNAsync(string isbn, string docId)
        {
            try
            {
                if (string.IsNullOrEmpty(isbn) || string.IsNullOrEmpty(docId))
                {
                    throw new ArgumentException("ISBN and docId must not be null or empty.");
                }

                await _redisDb.StringSetAsync(isbn, docId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error caching ISBN {isbn}: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveCachedISBNAsync(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                throw new ArgumentException("ISBN cannot be null or empty.", nameof(isbn));
            }

            await _redisDb.KeyDeleteAsync(isbn);
        }
    }
}
