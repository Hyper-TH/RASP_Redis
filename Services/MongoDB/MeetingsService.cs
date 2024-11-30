using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models.DatabaseSettings;
using RASP_Redis.Models.ProjectA;

namespace RASP_Redis.Services.MongoDB
{
    public class MeetingsService
    {
        private readonly IMongoCollection<Meeting> _meetingsCollection;

        public MeetingsService(IOptions<ProjectADatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _meetingsCollection = mongoDatabase.GetCollection<Meeting>(
                DatabaseSettings.Value.MeetingsCollectionName);
        }

        public async Task<List<Meeting>> GetAsync() =>
            await _meetingsCollection.Find(_ => true).ToListAsync();

        public async Task<Meeting?> GetAsync(string mid) =>
            await _meetingsCollection.Find(x => x.mID == mid).FirstOrDefaultAsync();

        public async Task<Meeting?> GetByMIDAsync(string mid) =>
            await _meetingsCollection.Find(Meeting => Meeting.mID == mid).FirstOrDefaultAsync();

        public async Task CreateAsync(Meeting newMeeting) =>
            await _meetingsCollection.InsertOneAsync(newMeeting);

        public async Task UpdateAsync(string mid, Meeting updatedMeeting) =>
            await _meetingsCollection.ReplaceOneAsync(x => x.mID == mid, updatedMeeting);

        public async Task RemoveAsync(string mid) =>
            await _meetingsCollection.DeleteOneAsync(x => x.mID == mid);
    }
}
