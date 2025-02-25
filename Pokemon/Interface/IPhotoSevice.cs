using CloudinaryDotNet.Actions;

namespace Pokemon.Interface
{
    public interface IPhotoSevice
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(String publicId);
          
    }
}
