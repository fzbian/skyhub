using skyhub.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace skyhub.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService()
        {
            string? connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
            string? database = Environment.GetEnvironmentVariable("MONGODB_DATABASE");
            if (connectionString == null || database == null)
                throw new Exception("Connection string or database name not found");

            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                // Perform some operation to check if the database is accessible
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
