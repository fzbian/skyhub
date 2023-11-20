using System.ComponentModel;
using System.Net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using skyhub.Models;

namespace skyhub.Services
{
    public interface ICloudinaryService
    {
        Task<Image> UploadImageAsync(User user, IFormFile file);
        Task<Image> GetImageByPublicId(Guid publicId);
        Task DeleteImageAsync(Guid publicId);
        Task<List<Image>> GetUserImages(string email);
    }

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            var acc = new Account(
                Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET"));

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<Image> UploadImageAsync(User user, IFormFile file)

        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream)
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            return new Image
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                UserId = user.Id,
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
        }

        public async Task<Image> GetImageByPublicId(Guid publicId)
        {
            var getResult = new GetResourceResult();
            var getParams = new GetResourceParams(publicId.ToString());

            getResult = await _cloudinary.GetResourceAsync(getParams);

            return new Image
            {
                Id = ObjectId.Parse(getResult.PublicId),
                Url = new Uri(getResult.SecureUrl).AbsoluteUri,
                UserId = ObjectId.Parse(getResult.PublicId),
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
        }

        public async Task DeleteImageAsync(Guid publicId)
        {
            var deleteParams = new DeletionParams(publicId.ToString());
            await _cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<List<Image>> GetUserImages(string email)
        {
            try
            {
                var getResult = await _cloudinary.GetResourceAsync(new GetResourceParams(email));

                if (getResult.StatusCode != HttpStatusCode.OK)
                {
                    return new List<Image>();
                }

                var image = new Image
                {
                    Id = ObjectId.Parse(getResult.PublicId),
                    Url = new Uri(getResult.SecureUrl).AbsoluteUri,
                    UserId = ObjectId.Parse(getResult.PublicId),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                return new List<Image> { image };
            }
            catch
            {
                return new List<Image>();
            }
        }
    }
}