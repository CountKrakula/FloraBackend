using System.Text.Json.Serialization;

namespace FloraBackend.DTOs.PlantSpecies
{
    public class PlantSpeciesListResponseDTO
    {

        [JsonPropertyName("data")]
        public List<PlantSpeciesListItemDTO> Data { get; set; } = [];
    }
}
