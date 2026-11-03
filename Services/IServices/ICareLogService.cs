using FloraBackend.DTOs.CareLogs;
using FloraBackend.DTOs.Pagination;

namespace FloraBackend.Services.IServices
{
    public interface ICareLogService
    {
        Task<PagedResult<CareLogDto>?> GetPagedByPlantAsync(int plantId, int userId, PaginationQuery query); Task<CareLogDto?> GetByIdAsync(int id, int plantId, int userId);
        Task<CareLogDto?> CreateAsync(CreateCareLogDto dto, int plantId, int userId);
        Task<bool> UpdateAsync(int id, UpdateCareLogDto dto, int plantId, int userId);
        Task<bool> DeleteAsync(int id, int plantId, int userId);
    }
}
