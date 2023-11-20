using skyhub.Models;
using skyhub.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace skyhub.Services
{
    public interface IAdminService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(string email, User user);
        Task DeleteAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task<List<Image>> GetAllImages();
    }

    public class AdminService : IAdminService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly ICloudinaryService _cloudinaryService;

        public AdminService(MongoDBService mongoDBService, ICloudinaryService cloudinaryService)
        {
            _userCollection = mongoDBService.GetCollection<User>("users");
            _cloudinaryService = cloudinaryService;
        }

        public async Task<User> CreateAsync(User user)
        {
            var newUser = new User
            {
                Id = ObjectId.GenerateNewId(),
                Name = user.Name,
                Password = PasswordService.HashPassword(user.Password),
                Email = user.Email,
                IsAdmin = user.IsAdmin,
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
                .Set(u => u.IsAdmin, user.IsAdmin)
                .Set(u => u.UpdatedAt, DateTime.Now);
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

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            User? user = await GetByEmailAsync(email);

            if (user != null && PasswordService.ValidatePassword(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public async Task<List<Image>> GetAllImages()
        {
            var users = await _userCollection.Find(_ => true).ToListAsync();
            var allImages = new List<Image>();
            foreach (var user in users)
            {
                if (user?.Images != null && user.Images.Any())
                {
                    allImages.AddRange(user.Images);
                }
            }
            return allImages;
        }
    }
}