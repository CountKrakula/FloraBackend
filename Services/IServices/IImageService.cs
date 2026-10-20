using FloraBackend.DTOs.Images;
using FloraBackend.Models;

namespace FloraBackend.Services.IServices
{
    public interface IImageService
    {
        Task<List<ImageDTO>> GetUserImages(int userId, int plantId);
        string? GetProfilePictureUrl(List<Image>? images);
        Task<(bool isSuccesful, List<string?> Errors)> CreateImage(IFormFile file, int userId, int plantId, bool isProfilePicture);
    }
}
