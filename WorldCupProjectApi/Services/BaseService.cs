using MongoDB.Driver;
using MongoDB.Bson;

namespace WorldCupProjectApi.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        protected BaseService(MongoDbService dbService, string collectionName)
        {
            _collection = dbService.GetCollection<T>(collectionName);
        }

       
        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

       
        public virtual async Task<T> GetByIdAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _collection.Find(Builders<T>.Filter.Eq("_id", objectId)).FirstOrDefaultAsync();
        }

       
        public virtual async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        
        public virtual async Task UpdateAsync(string id, T entity)
        {
            var objectId = ObjectId.Parse(id);
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", objectId), entity);
        }

       
        public virtual async Task DeleteAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", objectId));
        }

        
        public virtual async Task<long> CountAsync()
        {
            return await _collection.CountDocumentsAsync(_ => true);
        }

        
        public virtual async Task<bool> ExistsAsync(string id)
        {
            var objectId = ObjectId.Parse(id);
            return await _collection.Find(Builders<T>.Filter.Eq("_id", objectId)).AnyAsync();
        }
    }
}
