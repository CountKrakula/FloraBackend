using FloraBackend.Data;
using FloraBackend.DTOs.Pagination;
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

        public async Task<PagedResult<PlantHealthLog>> GetPagedForPlantAsync(int plantId, int userId, PaginationQuery query)
        {
            var baseQuery = _context.PlantHealthLogs
                .AsNoTracking()
                .Where(l => l.PlantId == plantId && l.Plant.UserId == userId)
                .OrderByDescending(l => l.RecordedAt);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<PlantHealthLog>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PlantHealthLog?> GetByIdAsync(int id, int userId)
        {
            return await _context.PlantHealthLogs
                .FirstOrDefaultAsync(l => l.PlantHealthLogId == id && l.Plant.UserId == userId);
        }

        public async Task<bool> UpdateAsync(PlantHealthLog log)
        {
            _context.PlantHealthLogs.Update(log);
            var updatedRows = await _context.SaveChangesAsync();
            return updatedRows > 0;
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