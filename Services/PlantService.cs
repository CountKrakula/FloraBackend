using FloraBackend.DTOs.Plants;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services.IServices;

namespace FloraBackend.Services
{
    public class PlantService : IPlantService
    {
        private readonly IPlantRepository _plantRepository;

        public PlantService(IPlantRepository plantRepository)
        {
            _plantRepository = plantRepository;
        }

        public async Task<List<PlantDto>> GetAllAsync(int userId)
        {
            var plants = await _plantRepository.GetAllByUserAsync(userId);
            return plants.Select(MapToDto).ToList();
        }

        public async Task<PlantDto?> GetByIdAsync(int id, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(id, userId);
            if (plant == null)
            {
                return null;
            }

            return MapToDto(plant);
        }

        public async Task<PlantDto> CreateAsync(CreatePlantDto dto, int userId)
        {
            var plant = new Plant
            {
                Name = dto.Name,
                Location = dto.Location,
                Notes = dto.Notes ?? string.Empty,
                PlantSpeciesId = dto.PlantSpeciesId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _plantRepository.AddAsync(plant);
            return MapToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePlantDto dto, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(id, userId);
            if (plant == null)
            {
                return false;
            }

            plant.Name = dto.Name;
            plant.Location = dto.Location;
            plant.Notes = dto.Notes ?? string.Empty;
            plant.PlantSpeciesId = dto.PlantSpeciesId;

            await _plantRepository.UpdateAsync(plant);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            return await _plantRepository.DeleteAsync(id, userId);
        }

        private static PlantDto MapToDto(Plant plant)
        {
            return new PlantDto
            {
                Id = plant.PlantId,
                Name = plant.Name,
                Location = plant.Location,
                Notes = plant.Notes,
                PlantSpeciesId = plant.PlantSpeciesId,
                CreatedAt = plant.CreatedAt
            };
        }
    }
}