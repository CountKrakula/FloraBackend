using FloraBackend.Data;
using FloraBackend.DTOs.Pagination;
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

        public async Task<PagedResult<CareLog>> GetPagedByPlantAsync(int plantId, PaginationQuery query)
        {
            var baseQuery = _context.CareLogs
                .AsNoTracking()
                .Where(log => log.PlantId == plantId)
                .OrderByDescending(log => log.Date);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<CareLog>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
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
