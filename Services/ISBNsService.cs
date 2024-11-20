using RASP_Redis.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RASP_Redis.Services
{
    public class ISBNsService
    {
        private readonly IMongoCollection<ISBN> _isbnsCollection;
        public ISBNsService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);
            
            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.ConnectionString);

            _isbnsCollection = mongoDatabase.GetCollection<ISBN>(
                bookStoreDatabaseSettings.Value.ISBNsCollectionName);
        }

        public async Task<List<ISBN>> GetAsync() =>
            await _isbnsCollection.Find(_ => true).ToListAsync();

        public async Task<ISBN?> GetAsync(string isbn) =>
            await _isbnsCollection.Find(x => x.Isbn == isbn).FirstOrDefaultAsync();

        public async Task CreateAsync(ISBN newISBN) =>
            await _isbnsCollection.InsertOneAsync(newISBN);

        public async Task RemoveAsync(string isbn) =>
            await _isbnsCollection.DeleteOneAsync(x => x.Isbn == isbn);
    }

}
