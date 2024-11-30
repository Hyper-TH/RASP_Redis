using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models.DatabaseSettings;
using RASP_Redis.Models.ProjectA;

namespace RASP_Redis.Services.MongoDB
{
    public class UserMeetingsService
    {
        private readonly IMongoCollection<UserMeetings> _userMeetingsCollection;

        public UserMeetingsService(IOptions<ProjectADatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _userMeetingsCollection = mongoDatabase.GetCollection<UserMeetings>(
                DatabaseSettings.Value.UserMeetingsCollectionName);
        }

        public async Task<List<UserMeetings>> GetAsync() =>
            await _userMeetingsCollection.Find(_ => true).ToListAsync();

        public async Task<UserMeetings?> GetAsync(string uid) =>
            await _userMeetingsCollection.Find(x => x.Id == uid).FirstOrDefaultAsync();

        public async Task<UserMeetings?> GetByUIDAsync(string uid) =>
            await _userMeetingsCollection.Find(User => User.Id == uid).FirstOrDefaultAsync();

        public async Task CreateAsync(UserMeetings newUserMeeting) =>
            await _userMeetingsCollection.InsertOneAsync(newUserMeeting);

        public async Task UpdateAsync(string uid, UserMeetings updatedUserMeeting) =>
            await _userMeetingsCollection.ReplaceOneAsync(x => x.Id == uid, updatedUserMeeting);

        public async Task RemoveAsync(string uid) =>
            await _userMeetingsCollection.DeleteOneAsync(x => x.Id == uid);

        public async Task AddMeetingAsync(string uID, string mID)
        {
            var updateDefinition = Builders<UserMeetings>.Update.Push(x => x.Meetings, mID);

            var result = await _userMeetingsCollection.UpdateOneAsync(
                x => x.Id == uID,
                updateDefinition
            );

            if (result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"No user found with ID {uID}");
            }

            if (result.ModifiedCount == 0)
            {
                Console.WriteLine($"No update made for user {uID}.");
            }
        }

        public async Task RemoveMeetingAsync(string uID, string mID)
        {
            var updateDefinition = Builders<UserMeetings>.Update.Pull(x => x.Meetings, mID);

            var result = await _userMeetingsCollection.UpdateOneAsync(
                x => x.Id == uID,
                updateDefinition
            );

            if (result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"No user found with ID {uID}");
            }

            if (result.ModifiedCount == 0)
            {
                Console.WriteLine($"No update made for user {uID}.");
            }
        }
    }
}
