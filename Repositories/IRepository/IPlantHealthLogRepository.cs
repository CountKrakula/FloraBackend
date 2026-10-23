using FloraBackend.Models;

namespace FloraBackend.Repositories.IRepository
{
    public interface IPlantHealthLogRepository
    {
        Task<List<PlantHealthLog>> GetAllForPlantAsync(int plantId, int userId);
        Task<PlantHealthLog?> GetByIdAsync(int id, int userId);
        Task<PlantHealthLog> AddAsync(PlantHealthLog log);
        Task<bool> DeleteAsync(int id, int userId);
    }
}