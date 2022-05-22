using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
         Task<ImageUploadResult> UploadImage(IFormFile file);

         Task<DeletionResult> DeleteImage(string publicId);
         
    }
}