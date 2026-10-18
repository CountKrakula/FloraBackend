namespace FloraBackend.DTOs.CareLogs
{
    public class CareLogDto
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string? Notes { get; set; }

        public int PlantId { get; set; }
    }
}