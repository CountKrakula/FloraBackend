using FloraBackend.DTOs.Pagination;
using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface IPlantHealthLogRepository
    {
        Task<PagedResult<PlantHealthLog>> GetPagedForPlantAsync(int plantId, int userId, PaginationQuery query);
        Task<PlantHealthLog?> GetByIdAsync(int id, int userId);
        Task<PlantHealthLog> AddAsync(PlantHealthLog log);
        Task<bool> UpdateAsync(PlantHealthLog log);
        Task<bool> DeleteAsync(int id, int userId);
    }
}