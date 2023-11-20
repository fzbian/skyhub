using skyhub.Models;
using skyhub.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace skyhub.Services
{
    public interface IUserService
    {
        Task<Image> CreateImage(User user, IFormFile file);
        Task<Image> GetImageByPublicId(Guid publicId);
        Task DeleteImageAsync(Guid publicId);
        Task<List<Image>> GetUserImages(string email);
    }

    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Image> _imageCollection;
        private readonly ICloudinaryService _cloudinaryService;

        public UserService(MongoDBService mongoDBService, ICloudinaryService cloudinaryService)
        {
            _userCollection = mongoDBService.GetCollection<User>("users");
            _imageCollection = mongoDBService.GetCollection<Image>("images");
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Image> CreateImage(User user, IFormFile file)
        {
            var image = await _cloudinaryService.UploadImageAsync(user, file);
            await _userCollection.UpdateOneAsync(u => u.Id == user.Id,
                Builders<User>.Update.Push(u => u.Images, image));
            await _imageCollection.InsertOneAsync(image);
            return image;
        }

        public async Task<Image> GetImageByPublicId(Guid publicId)
        {
            return await _imageCollection.Find(i => i.PublicId == publicId).FirstOrDefaultAsync();
        }

        public async Task DeleteImageAsync(Guid publicId)
        {
            await _userCollection.UpdateOneAsync(u => u.Images.Any(i => i.PublicId == publicId),
                Builders<User>.Update.PullFilter(u => u.Images, Builders<Image>.Filter.Eq(i => i.PublicId, publicId)));
            await _imageCollection.DeleteOneAsync(i => i.PublicId == publicId);
            await _cloudinaryService.DeleteImageAsync(publicId);
        }

        public async Task<List<Image>> GetUserImages(string email)
        {
            var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user?.Images == null || !user.Images.Any())
            {
                return new List<Image>();
            }
            return user.Images;
        }
    }
}