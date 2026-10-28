using FloraBackend.Data;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FloraBackend.Repositories
{
    public class CareLogRepository : ICareLogRepository
    {
        private readonly FloraDBContext _context;

        public CareLogRepository(FloraDBContext context)
        {
            _context = context;
        }

        public async Task<List<CareLog>> GetAllByPlantAsync(int plantId)
        {
            return await _context.CareLogs
                .Where(log => log.PlantId == plantId)
                .OrderByDescending(log => log.Date)
                .ToListAsync();
        }

        public async Task<CareLog?> GetByIdAsync(int id, int plantId)
        {
            return await _context.CareLogs
                .FirstOrDefaultAsync(log =>
                    log.Id == id && log.PlantId == plantId);
        }

        public async Task<CareLog> AddAsync(CareLog careLog)
        {
            _context.CareLogs.Add(careLog);
            await _context.SaveChangesAsync();
            return careLog;
        }

        public async Task UpdateAsync(CareLog careLog)
        {
            _context.CareLogs.Update(careLog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id, int plantId)
        {
            var careLog = await GetByIdAsync(id, plantId);

            if (careLog == null)
            {
                return false;
            }

            _context.CareLogs.Remove(careLog);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
