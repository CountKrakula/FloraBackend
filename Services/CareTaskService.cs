using FloraBackend.DTOs.CareTasks;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services.IServices;

namespace FloraBackend.Services
{
    public class CareTaskService : ICareTaskService
    {
        private readonly ICareTaskRepository _careTaskRepository;
        private readonly IPlantRepository _plantRepository;

        public CareTaskService(
            ICareTaskRepository careTaskRepository,
            IPlantRepository plantRepository)
        {
            _careTaskRepository = careTaskRepository;
            _plantRepository = plantRepository;
        }

        public async Task<List<CareTaskDto>> GetAllByPlantAsync(
            int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return new List<CareTaskDto>();
            }

            var careTasks = await _careTaskRepository.GetAllByPlantAsync(plantId);
            return careTasks.Select(MapToDto).ToList();
        }

        public async Task<CareTaskDto?> GetByIdAsync(
            int id, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return null;
            }

            var careTask = await _careTaskRepository.GetByIdAsync(id, plantId);

            return careTask == null ? null : MapToDto(careTask);
        }

        public async Task<CareTaskDto?> CreateAsync(
            CreateCareTaskDto dto, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null || dto.PlantId != plantId)
            {
                return null;
            }

            if (!Enum.TryParse<CareTaskType>(
                dto.Type, true, out var careTaskType))
            {
                throw new ArgumentException("Invalid care task type.");
            }

            if (dto.IntervalDays < 1 || dto.IntervalDays > 365)
            {
                throw new ArgumentException(
                    "IntervalDays must be between 1 and 365.");
            }

            var careTask = new CareTask
            {
                PlantId = plantId,
                Type = careTaskType,
                IntervalDays = dto.IntervalDays,
                NextDueAt = DateTime.UtcNow.AddDays(dto.IntervalDays)
            };

            var created = await _careTaskRepository.AddAsync(careTask);

            return MapToDto(created);
        }

        public async Task<bool> UpdateAsync(
            int id, UpdateCareTaskDto dto, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return false;
            }

            if (!Enum.TryParse<CareTaskType>(
                dto.Type, true, out var careTaskType))
            {
                throw new ArgumentException("Invalid care task type.");
            }

            if (dto.IntervalDays < 1 || dto.IntervalDays > 365)
            {
                throw new ArgumentException(
                    "IntervalDays must be between 1 and 365.");
            }

            var careTask = await _careTaskRepository.GetByIdAsync(id, plantId);

            if (careTask == null)
            {
                return false;
            }

            careTask.Type = careTaskType;
            careTask.IntervalDays = dto.IntervalDays;
            careTask.NextDueAt = (careTask.LastDoneAt ?? DateTime.UtcNow)
                .AddDays(dto.IntervalDays);

            await _careTaskRepository.UpdateAsync(careTask);

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

            return await _careTaskRepository.DeleteAsync(id, plantId);
        }

        public async Task<CareTaskDto?> CompleteAsync(int id, int plantId, int userId)
        {
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);

            if (plant == null)
            {
                return null;
            }

            var careTask = await _careTaskRepository.GetByIdAsync(id, plantId);

            if (careTask == null)
            {
                return null;
            }

            // LastDoneAt and NextDueAt will use the exact same present time
            var now = DateTime.UtcNow;
            
            careTask.LastDoneAt = now;

            careTask.NextDueAt = now.AddDays(careTask.IntervalDays);

            // Use the repository update function to save the updated task to the database
            await _careTaskRepository.UpdateAsync(careTask);

            return MapToDto(careTask);
        }

        private static CareTaskDto MapToDto(CareTask careTask)
        {
            return new CareTaskDto
            {
                Id = careTask.Id,
                Type = careTask.Type.ToString(),
                IntervalDays = careTask.IntervalDays,
                LastDoneAt = careTask.LastDoneAt,
                NextDueAt = careTask.NextDueAt,
                PlantId = careTask.PlantId
            };
        }
    }
}


