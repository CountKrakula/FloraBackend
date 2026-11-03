using FloraBackend.Data;
using FloraBackend.DTOs.Pagination;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace FloraBackend.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly FloraDBContext _context;

        public PlantRepository(FloraDBContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Plant>> GetPagedByUserAsync(int userId, PaginationQuery query)
        {
            var baseQuery = _context.Plants
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Plant>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Plant?> GetByIdAsync(int id, int userId)
        {
            return await _context.Plants
                .FirstOrDefaultAsync(p => p.PlantId == id && p.UserId == userId);
        }

        public async Task<Plant> AddAsync(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            return plant;
        }

        public async Task UpdateAsync(Plant plant)
        {
            _context.Plants.Update(plant);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var plant = await GetByIdAsync(id, userId);
            if (plant == null)
            {
                return false;
            }
            
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}