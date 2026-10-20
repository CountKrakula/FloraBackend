
using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface ICareTaskRepository
    {
        Task<List<CareTask>> GetAllByPlantAsync(int plantId);
        Task<CareTask?> GetByIdAsync(int id, int plantId);
        Task<CareTask> AddAsync(CareTask careTask);
        Task UpdateAsync(CareTask careTask);
        Task<bool> DeleteAsync(int id, int plantId);
    }
}
