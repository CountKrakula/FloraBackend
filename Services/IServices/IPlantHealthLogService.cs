using FloraBackend.DTOs.PlantHealthLogs;

namespace FloraBackend.Services.IServices
{
    public interface IPlantHealthLogService
    {
        Task<List<PlantHealthLogDto>> GetAllForPlantAsync(int plantId, int userId);
        Task<PlantHealthLogDto?> CreateAsync(CreatePlantHealthLogDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdatePlantHealthLogDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}