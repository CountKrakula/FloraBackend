using FloraBackend.DTOs.Pagination;
using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface IPlantRepository
    {
        Task<PagedResult<Plant>> GetPagedByUserAsync(int userId, PaginationQuery query);
        Task<Plant?> GetByIdAsync(int id, int userId);
        Task<Plant> AddAsync(Plant plant);
        Task UpdateAsync(Plant plant);
        Task<bool> DeleteAsync(int id, int userId);
    }
}