using FloraBackend.DTOs.CareLogs;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services.IServices;

namespace FloraBackend.Services
{
    public class CareLogService : ICareLogService
    {
        private readonly ICareLogRepository _careLogRepository;
        private readonly IPlantRepository _plantRepository;

        public CareLogService(
            ICareLogRepository careLogRepository,
            IPlantRepository plantRepository)
        {
            _careLogRepository = careLogRepository;
            _plantRepository = plantRepository;
        }

        public async Task<List<CareLogDto>?> GetAllByPlantAsync(
            int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return null;
            }

            var careLogs =
                await _careLogRepository.GetAllByPlantAsync(plantId);

            return careLogs.Select(MapToDto).ToList();
        }

        public async Task<CareLogDto?> GetByIdAsync(
            int id, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return null;
            }

            var careLog =
                await _careLogRepository.GetByIdAsync(id, plantId);

            return careLog == null ? null : MapToDto(careLog);
        }

        public async Task<CareLogDto?> CreateAsync(
            CreateCareLogDto dto, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return null;
            }

            var careLog = new CareLog
            {
                Date = dto.Date,
                Notes = dto.Notes,
                PlantId = plantId
            };

            var created = await _careLogRepository.AddAsync(careLog);

            return MapToDto(created);
        }

        public async Task<bool> UpdateAsync(
            int id, UpdateCareLogDto dto, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return false;
            }

            var careLog =
                await _careLogRepository.GetByIdAsync(id, plantId);

            if (careLog == null)
            {
                return false;
            }

            careLog.Date = dto.Date;
            careLog.Notes = dto.Notes;

            await _careLogRepository.UpdateAsync(careLog);

            return true;
        }

        public async Task<bool> DeleteAsync(
            int id, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return false;
            }

            return await _careLogRepository.DeleteAsync(id, plantId);
        }

        private static CareLogDto MapToDto(CareLog careLog)
        {
            return new CareLogDto
            {
                Id = careLog.Id,
                Date = careLog.Date,
                Notes = careLog.Notes,
                PlantId = careLog.PlantId
            };
        }
    }
}
