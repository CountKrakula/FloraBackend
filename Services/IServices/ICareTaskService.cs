using FloraBackend.DTOs.CareTasks;

namespace FloraBackend.Services.IServices
{
    public interface ICareTaskService
    {
        Task<List<CareTaskDto>> GetAllByPlantAsync(int plantId, int userId);
        Task<CareTaskDto?> GetByIdAsync(int id, int plantId, int userId);
        Task<CareTaskDto?> CreateAsync(CreateCareTaskDto dto, int plantId, int userId);
        Task<bool> UpdateAsync(int id, UpdateCareTaskDto dto, int plantId, int userId);
        Task<bool> DeleteAsync(int id, int plantId, int userId);
    }
}