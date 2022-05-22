using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.SecretKey
            );

            _cloudinary = new Cloudinary(acc);

        }

        public async Task<DeletionResult> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams); 

            return result;
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            var AddPhotoAsync = new ImageUploadResult();

            if(file.Length > 0)
            {

                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                AddPhotoAsync = await _cloudinary.UploadAsync(uploadParams);
            }

            return AddPhotoAsync;
        }
    }
}