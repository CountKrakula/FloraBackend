namespace FloraBackend.DTOs.PlantHealthLogs
{
    public class PlantHealthLogDto
    {
        public int Id { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime RecordedAt { get; set; }

        public int PlantId { get; set; }
    }
}