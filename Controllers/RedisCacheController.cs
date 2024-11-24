using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RASP_Redis.Controllers
{
    public class RedisCacheController : Controller
    {
        private readonly IDistributedCache _cache;

        public RedisCacheController(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            // Setting a cache value
            await _cache.SetStringAsync("myKey", "myValue", new DistributedCacheEntryOptions());

            // Retrieving a cache value
            var value = await _cache.GetStringAsync("myKey");

            return View("Index", value);
        }
    }
}
