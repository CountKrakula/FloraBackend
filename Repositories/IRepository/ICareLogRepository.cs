using FloraBackend.DTOs.Pagination;
using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface ICareLogRepository
    {
        Task<PagedResult<CareLog>> GetPagedByPlantAsync(int plantId, PaginationQuery query);
        Task<CareLog?> GetByIdAsync(int id, int plantId);
        Task<CareLog> AddAsync(CareLog careLog);
        Task UpdateAsync(CareLog careLog);
        Task<bool> DeleteAsync(int id, int plantId);
    }
}
