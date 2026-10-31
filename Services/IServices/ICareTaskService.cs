using FloraBackend.DTOs.CareTasks;

namespace FloraBackend.Services.IServices
{
    public interface ICareTaskService
    {
        Task<List<CareTaskDto>> GetAllByPlantAsync(int plantId, int userId);
        Task<CareTaskDto?> GetByIdAsync(int id, int plantId, int userId);
        Task<CareTaskDto?> CreateAsync(CreateCareTaskDto dto, int plantId, int userId);
        Task<bool> UpdateAsync(int id, UpdateCareTaskDto dto, int plantId, int userId);
        Task<bool> DeleteAsync(int id, int plantId, int userId);
        // Marks a task as done: sets LastDoneAt to now and NextDueAt to now + IntervalDays.
        // Returns the updated task, or null if the task or plant is not found.
        Task<CareTaskDto?> CompleteAsync(int id, int plantId, int userId);
    }
}