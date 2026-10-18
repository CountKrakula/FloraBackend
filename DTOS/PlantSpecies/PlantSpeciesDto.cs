
namespace FloraBackend.DTOs.PlantSpecies
{
    public class PlantSpeciesDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int WateringIntervalDays { get; set; }

        public string? Description { get; set; }

        public string? CareInstructions { get; set; }

        public string LightRequirement { get; set; } = string.Empty;
    }
}
