namespace FloraBackend.DTOs.Plants
{
    public class PlantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int PlantSpeciesId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
