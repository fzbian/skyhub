using skyhub.Models;
using skyhub.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace skyhub.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(MongoDBService mongoDBService)
        {
            _userCollection = mongoDBService.GetCollection<User>("users");
        }

        public async Task<User> CreateAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            await _userCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task DeleteAsync(string id)
        {
            await _userCollection.DeleteOneAsync(u => u.Id == ObjectId.Parse(id));
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _userCollection.Find(u => u.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userCollection.Find(u => true).ToListAsync();
        }
    }
}