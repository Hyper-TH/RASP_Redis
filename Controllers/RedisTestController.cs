using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisTestController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public RedisTestController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("/test-redis")]
        public async Task<IActionResult> TestRedis()
        {
            try
            {
                // Set a value in Redis
                await _cache.SetStringAsync("TestKey", "TestValue");

                // Get the value from Redis
                var value = await _cache.GetStringAsync("TestKey");

                return Ok($"Redis connection successful! Value: {value}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Redis connection failed: {ex.Message}");
            }
        }
    }
}
