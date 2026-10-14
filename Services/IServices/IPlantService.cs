using FloraBackend.DTOs;

namespace FloraBackend.Services.IServices
{
    public interface IPlantService
    {
        Task<List<PlantDto>> GetAllAsync(int userId);
        Task<PlantDto?> GetByIdAsync(int id, int userId);
        Task<PlantDto> CreateAsync(CreatePlantDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdatePlantDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}