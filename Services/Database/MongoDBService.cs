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
            if (connectionString == null)
                throw new Exception("Connection string not found");

            MongoClient client = new MongoClient(connectionString);
            _database = client.GetDatabase("skyhub");
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
