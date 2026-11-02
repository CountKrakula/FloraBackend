using FloraBackend.DTOs.Pagination;
using FloraBackend.DTOs.Plants;

namespace FloraBackend.Services.IServices
{
    public interface IPlantService
    {
        Task<PagedResult<PlantDto>> GetPagedAsync(int userId, PaginationQuery query);
        Task<PlantDto?> GetByIdAsync(int id, int userId);
        Task<PlantDto> CreateAsync(CreatePlantDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdatePlantDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}