namespace FloraBackend.DTOs.Plants
{
    public class UpdatePlantDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
