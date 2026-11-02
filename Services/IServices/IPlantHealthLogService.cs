using FloraBackend.DTOs.Pagination;
using FloraBackend.DTOs.PlantHealthLogs;

namespace FloraBackend.Services.IServices
{
    public interface IPlantHealthLogService
    {
        Task<PagedResult<PlantHealthLogDto>> GetPagedForPlantAsync(int plantId, int userId, PaginationQuery query); Task<PlantHealthLogDto?> CreateAsync(CreatePlantHealthLogDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdatePlantHealthLogDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}