using skyhub.Models;
using skyhub.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace skyhub.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(string email, User user);
        Task DeleteAsync(string email);
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
            var newUser = new User
            {
                Id = ObjectId.GenerateNewId(),
                Name = user.Name,
                Password = PasswordService.HashPassword(user.Password),
                Email = user.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                Images = new List<Image>()
                
            };
            await _userCollection.InsertOneAsync(newUser);
            return newUser;
        }

        public async Task<User> UpdateAsync(string email, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var update = Builders<User>.Update
                .Set(u => u.Name, user.Name)
                .Set(u => u.Password, PasswordService.HashPassword(user.Password))
                .Set(u => u.Email, user.Email)
                .Set(u => u.UpdatedAt, DateTime.Now);
                //.Set(u => u.Images, user.Images);
            await _userCollection.UpdateOneAsync(filter, update);
            return user;
        }

        public async Task DeleteAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            await _userCollection.DeleteOneAsync(filter);
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