using MongoDB.Driver;
using WorldCupProjectApi.Models;

namespace WorldCupProjectApi.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("WorldCupProject");
        }
        
        
        // Generic method for BaseService to use
        public IMongoCollection<T> GetCollection<T>(string collectionName) where T : class
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}    