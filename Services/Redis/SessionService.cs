using StackExchange.Redis;
using RASP_Redis.Models.Auth;

namespace RASP_Redis.Services.Redis
{
    public interface ISessionService
    {
        Task SetSessionAsync(string token, User user);
        Task<User> GetSessionAsync(string token);
        Task RemoveSessionAsync(string token);
    }
    public class SessionService : ISessionService
    {
        private readonly IConnectionMultiplexer _redis;

        public SessionService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task SetSessionAsync(string token, User user)
        {
            var db = _redis.GetDatabase();
            var userJson = Newtonsoft.Json.JsonConvert.SerializeObject(user);

            await db.StringSetAsync(token, userJson, TimeSpan.FromMinutes(30));
        }

        public async Task<User> GetSessionAsync(string token)
        {
            var db = _redis.GetDatabase();
            var userJson = await db.StringGetAsync(token);

            return userJson.IsNullOrEmpty ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
        }

        public async Task RemoveSessionAsync(string token)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(token);
        }

    }
}
