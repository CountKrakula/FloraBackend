using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface ICareLogRepository
    {
        Task<List<CareLog>> GetAllByPlantAsync(int plantId);
        Task<CareLog?> GetByIdAsync(int id, int plantId);
        Task<CareLog> AddAsync(CareLog careLog);
        Task UpdateAsync(CareLog careLog);
        Task<bool> DeleteAsync(int id, int plantId);
    }
}
