

namespace FloraBackend.DTOs.CareTasks
{
    public class CreateCareTaskDto
    {
        public int PlantId { get; set; }

        public string Type { get; set; } = string.Empty;

        public int IntervalDays { get; set; }
    }
}
