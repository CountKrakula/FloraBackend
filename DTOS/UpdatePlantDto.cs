namespace FloraBackend.DTOs
{
    public class UpdatePlantDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int PlantSpeciesId { get; set; }
    }
}
