using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models.DatabaseSettings;
using RASP_Redis.Models.Auth;

namespace RASP_Redis.Services.MongoDB
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService(IOptions<ProjectADatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                DatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() => 
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string uid) =>
            await _usersCollection.Find(x => x.UID == uid).FirstOrDefaultAsync();

        public async Task<User?> GetByUIDAsync(string uid) =>
            await _usersCollection.Find(User => User.UID == uid).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string uid, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.UID == uid, updatedUser);

        public async Task RemoveAsync(string uid) =>
            await _usersCollection.DeleteOneAsync(x => x.UID == uid);
    }
}
