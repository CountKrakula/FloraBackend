using FloraBackend.Data;
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

        public async Task<List<Plant>> GetAllByUserAsync(int userId)
        {
            return await _context.Plants
                .Where(p => p.UserId == userId)
                .ToListAsync();
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