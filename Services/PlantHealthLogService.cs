using FloraBackend.DTOs.Pagination;
using FloraBackend.DTOs.PlantHealthLogs;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services.IServices;

namespace FloraBackend.Services
{
    public class PlantHealthLogService : IPlantHealthLogService
    {
        private readonly IPlantHealthLogRepository _healthLogRepository;
        private readonly IPlantRepository _plantRepository;

        public PlantHealthLogService(
            IPlantHealthLogRepository healthLogRepository,
            IPlantRepository plantRepository)
        {
            _healthLogRepository = healthLogRepository;
            _plantRepository = plantRepository;
        }

        public async Task<PagedResult<PlantHealthLogDto>> GetPagedForPlantAsync(int plantId, int userId, PaginationQuery query)
        {
            var pagedLogs = await _healthLogRepository.GetPagedForPlantAsync(plantId, userId, query);

            return new PagedResult<PlantHealthLogDto>
            {
                Items = pagedLogs.Items.Select(MapToDto).ToList(),
                PageNumber = pagedLogs.PageNumber,
                PageSize = pagedLogs.PageSize,
                TotalCount = pagedLogs.TotalCount
            };
        }

        public async Task<PlantHealthLogDto?> CreateAsync(CreatePlantHealthLogDto dto, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(dto.PlantId, userId);
            if (plant == null)
            {
                return null;
            }

            var log = new PlantHealthLog
            {
                PlantId = dto.PlantId,
                Status = dto.Status,
                RecordedAt = DateTime.UtcNow
            };

            var created = await _healthLogRepository.AddAsync(log);
            return MapToDto(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePlantHealthLogDto dto, int userId)
        {
            var log = await _healthLogRepository.GetByIdAsync(id, userId);
            if (log == null)
            {
                return false;
            }

            log.Status = dto.Status;

            return await _healthLogRepository.UpdateAsync(log);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            return await _healthLogRepository.DeleteAsync(id, userId);
        }

        private static PlantHealthLogDto MapToDto(PlantHealthLog log)
        {
            return new PlantHealthLogDto
            {
                Id = log.PlantHealthLogId,
                Status = log.Status,
                RecordedAt = log.RecordedAt,
                PlantId = log.PlantId
            };
        }
    }
}