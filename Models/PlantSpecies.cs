using System.ComponentModel.DataAnnotations;

namespace FloraBackend.Models;

public class PlantSpecies
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 365)]
    public int WateringIntervalDays { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(2000)]
    public string? CareInstructions { get; set; }

    public LightRequirement LightRequirement { get; set; }

    public List<Plant> Plants { get; set; } = new();
}

public enum LightRequirement
{
    Low,
    Medium,
    Bright,
    DirectSun
}