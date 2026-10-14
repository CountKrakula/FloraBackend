using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface IPlantRepository
    {
        Task<List<Plant>> GetAllByUserAsync(int userId);
        Task<Plant?> GetByIdAsync(int id, int userId);
        Task<Plant> AddAsync(Plant plant);
        Task UpdateAsync(Plant plant);
        Task<bool> DeleteAsync(int id, int userId);
    }
}