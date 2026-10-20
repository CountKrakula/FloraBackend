using FloraBackend.Models;
namespace FloraBackend.Repositories.IRepository
{
    public interface IImageRepository
    {
        Task<Image> GetImage(int imgId);
        Task<List<Image>> GetImageByUserId(int userId, int? plantId);
        Task<List<Image>> GetImageByPlantId(int plantId);
        Task<int> CreateImage(Image image);
        Task<bool> ImageExistsByHashAndPlant(string hash, int plantId);
        Task ClearProfilePicturesForPlant(int plantId);
    }
}
