using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RASP_Redis.Models.DatabaseSettings;
using System.Security.Cryptography;
using System;
using RASP_Redis.Models.ProjectA;

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
        public async Task RemoveAsync(string mid) =>
          await _AttendeesCollection.DeleteOneAsync(x => x.Id == mid);

        public async Task AddUserToMeetingAsync(string uid, string mid)
        {
            var filter = Builders<Attendees>.Filter.Eq(x => x.Id, mid); 
            var update = Builders<Attendees>.Update.AddToSet(x => x.Users, uid); 

            var result = await _AttendeesCollection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"Meeting with ID {mid} not found.");
            }

            if (result.ModifiedCount == 0)
            {
                // If no modification, uID might already be in the list
                Console.WriteLine($"User {uid} is already in the meeting {mid}.");
            }
        }

        public async Task RemoveOneAsync(string uid, string mid)
        {
            var filter = Builders<Attendees>.Filter.Eq(x => x.Id, mid); 
            var update = Builders<Attendees>.Update.Pull(x => x.Users, uid); 

            var result = await _AttendeesCollection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                throw new InvalidOperationException($"Meeting with ID {mid} not found.");
            }

            if (result.ModifiedCount == 0)
            {
                Console.WriteLine($"User {uid} not found in meeting {mid}.");
            }
        }
    }
}
