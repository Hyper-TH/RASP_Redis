using RASP_Redis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models.DatabaseSettings;

namespace RASP_Redis.Services.MongoDB
{
    public class AttendeesService
    {
        private readonly IMongoCollection<Attendees> _AttendeesCollection;

        public AttendeesService(IOptions<ProjectADatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _AttendeesCollection = mongoDatabase.GetCollection<Attendees>(
                DatabaseSettings.Value.AttendeesCollectionName);
        }

        public async Task<List<Attendees>> GetAsync() =>
            await _AttendeesCollection.Find(_ => true).ToListAsync();

        public async Task<Attendees?> GetAsync(string mid) =>
            await _AttendeesCollection.Find(x => x.Id == mid).FirstOrDefaultAsync();

        public async Task<Attendees?> GetByIDAsync(string mid) =>
            await _AttendeesCollection.Find(Attendees => Attendees.Id == mid).FirstOrDefaultAsync();

        public async Task CreateAsync(Attendees newAttendees) =>
            await _AttendeesCollection.InsertOneAsync(newAttendees);

        public async Task UpdateAsync(string mid, Attendees updatedAttendees) =>
            await _AttendeesCollection.ReplaceOneAsync(x => x.Id == mid, updatedAttendees);

        public async Task RemoveAsync(string mid) =>
            await _AttendeesCollection.DeleteOneAsync(x => x.Id == mid);
    }
}
