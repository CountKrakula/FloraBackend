using FloraBackend.DTOs.CareLogs;

namespace FloraBackend.Services.IServices
{
    public interface ICareLogService
    {
        Task<List<CareLogDto>> GetAllByPlantAsync(int plantId, int userId);
        Task<CareLogDto?> GetByIdAsync(int id, int plantId, int userId);
        Task<CareLogDto?> CreateAsync(CareLogDto dto, int plantId, int userId);
        Task<bool> UpdateAsync(int id, CareLogDto dto, int plantId, int userId);
        Task<bool> DeleteAsync(int id, int plantId, int userId);
    }
}