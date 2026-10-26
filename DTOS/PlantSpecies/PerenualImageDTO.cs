using System.Text.Json.Serialization;

namespace FloraBackend.DTOs.PlantSpecies
{
    public class PerenualImageDTO
    {
        [JsonPropertyName("regular_url")]
        public string? RegularUrl { get; set; }
    }
}
