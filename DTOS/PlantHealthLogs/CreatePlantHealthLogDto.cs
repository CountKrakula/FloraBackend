using System.ComponentModel.DataAnnotations;

namespace FloraBackend.DTOs.PlantHealthLogs
{
    public class CreatePlantHealthLogDto
    {
        [Required]
        public int PlantId { get; set; }

        [Required]
        [AllowedValues("Healthy", "Sick", "Yellow Leaves", "Brown Leaves", "Bugs")]
        public string Status { get; set; } = string.Empty;
    }
}