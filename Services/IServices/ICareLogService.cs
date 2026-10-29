using FloraBackend.DTOs.CareLogs;

namespace FloraBackend.Services.IServices
{
    public interface ICareLogService
    {
        Task<List<CareLogDto>?> GetAllByPlantAsync(int plantId, int userId);
        Task<CareLogDto?> GetByIdAsync(int id, int plantId, int userId);
        Task<CareLogDto?> CreateAsync(CreateCareLogDto dto, int plantId, int userId);
        Task<bool> UpdateAsync(int id, UpdateCareLogDto dto, int plantId, int userId);
        Task<bool> DeleteAsync(int id, int plantId, int userId);
    }
}
