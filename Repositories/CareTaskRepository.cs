
using FloraBackend.Data;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FloraBackend.Repositories
{
    public class CareTaskRepository : ICareTaskRepository
    {
        private readonly FloraDBContext _context;

        public CareTaskRepository(FloraDBContext context)
        {
            _context = context;
        }

        public async Task<List<CareTask>> GetAllByPlantAsync(int plantId)
        {
            return await _context.CareTasks
                .Where(ct => ct.PlantId == plantId)
                .ToListAsync();
        }

        public async Task<CareTask?> GetByIdAsync(int id, int plantId)
        {
            return await _context.CareTasks
                .FirstOrDefaultAsync(ct =>
                    ct.Id == id && ct.PlantId == plantId);
        }

        public async Task<CareTask> AddAsync(CareTask careTask)
        {
            _context.CareTasks.Add(careTask);
            await _context.SaveChangesAsync();

            return careTask;
        }

        public async Task UpdateAsync(CareTask careTask)
        {
            _context.CareTasks.Update(careTask);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id, int plantId)
        {
            var careTask = await GetByIdAsync(id, plantId);

            if (careTask == null)
            {
                return false;
            }

            _context.CareTasks.Remove(careTask);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

