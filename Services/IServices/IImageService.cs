using FloraBackend.DTOs.Images;
using FloraBackend.Models;

namespace FloraBackend.Services.IServices
{
    public interface IImageService
    {
        Task<List<ImageDTO>> GetUserImages(int userId, int plantId);
        Task<(bool isSuccesful, List<string?> Errors)> CreateImage(IFormFile file, int userId, int plantId, bool isProfilePicture);
        Task<(bool isSuccessful, List<string?> Errors)> UpdateImage(int imageId, IFormFile newFile, int userId);
        Task<(bool isSuccessful, List<string?> Errors)> DeleteImage(int imageId, int userId);
    }
}
