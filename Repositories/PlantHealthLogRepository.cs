using FloraBackend.Data;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FloraBackend.Repositories
{
    public class PlantHealthLogRepository : IPlantHealthLogRepository
    {
        private readonly FloraDBContext _context;

        public PlantHealthLogRepository(FloraDBContext context)
        {
            _context = context;
        }

        public async Task<List<PlantHealthLog>> GetAllForPlantAsync(int plantId, int userId)
        {
            return await _context.PlantHealthLogs
                .Where(l => l.PlantId == plantId && l.Plant.UserId == userId)
                .OrderByDescending(l => l.RecordedAt)
                .ToListAsync();
        }

        public async Task<PlantHealthLog?> GetByIdAsync(int id, int userId)
        {
            return await _context.PlantHealthLogs
                .FirstOrDefaultAsync(l => l.PlantHealthLogId == id && l.Plant.UserId == userId);
        }

        public async Task<PlantHealthLog> AddAsync(PlantHealthLog log)
        {
            _context.PlantHealthLogs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var log = await GetByIdAsync(id, userId);
            if (log == null)
            {
                return false;
            }

            _context.PlantHealthLogs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}