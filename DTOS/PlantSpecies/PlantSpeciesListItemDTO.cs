using System.Text.Json.Serialization;

namespace FloraBackend.DTOs.PlantSpecies
{
    public class PlantSpeciesListItemDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("common_name")]
        public string? CommonName { get; set; }

        [JsonPropertyName("scientific_name")]
        public List<string>? ScientificName { get; set; }

        [JsonPropertyName("default_image")]
        public PerenualImageDTO? DefaultImage { get; set; }
    }
}
