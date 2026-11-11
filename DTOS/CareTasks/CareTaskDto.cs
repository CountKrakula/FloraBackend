

namespace FloraBackend.DTOs.CareTasks
{
    public class CareTaskDto
    {
        public int Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public int IntervalDays { get; set; }

        public DateTime? LastDoneAt { get; set; }

        public DateTime NextDueAt { get; set; }

        public int PlantId { get; set; }

        public string PlantName { get; set; } = string.Empty;
    }
}


