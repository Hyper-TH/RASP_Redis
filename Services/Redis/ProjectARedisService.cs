using Microsoft.Extensions.Caching.Distributed;

namespace RASP_Redis.Services.Redis
{
    public class ProjectARedisService
    {
        private readonly IDistributedCache _cache;

        public ProjectARedisService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetProjectACacheAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions());
        }

        public async Task<string?> GetProjectACacheAsync(string key)
        {
            return await _cache.GetStringAsync(key);
        }
    }

}
