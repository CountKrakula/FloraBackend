using System.Text.Json.Serialization;

namespace FloraBackend.DTOs.PlantSpecies
{
    public class PlantSpeciesDetailsDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("common_name")]
        public string? CommonName { get; set; }

        [JsonPropertyName("scientific_name")]
        public List<string>? ScientificName { get; set; }

        public string? Description { get; set; }

        public string? Watering { get; set; }

        [JsonPropertyName("watering_general_benchmark")]
        public PerenualWateringDTO? WateringGeneralBenchmark { get; set; }

        public List<string>? Sunlight { get; set; }

        [JsonPropertyName("care_level")]
        public string? CareLevel { get; set; }

        public bool? Indoor { get; set; }

        [JsonPropertyName("poisonous_to_pets")]
        public bool? PoisonousToPets { get; set; }

        [JsonPropertyName("default_image")]
        public PerenualImageDTO? DefaultImage { get; set; }
    }
}
