using skyhub.Models;
using MongoDB.Driver;

namespace skyhub.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Image> _imageCollection;

        public MongoDBService()
        {
            string? connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
            if (connectionString == null)
                throw new Exception("Connection string not found");

            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("skyhub");
            _userCollection = database.GetCollection<User>("users");
            _imageCollection = database.GetCollection<Image>("images");
        }

        public async Task<bool> PingAsync()
        {
            try
            {
                await _userCollection.Find(user => true).FirstOrDefaultAsync();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}
