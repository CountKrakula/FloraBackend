using System.ComponentModel.DataAnnotations;

namespace FloraBackend.DTOs.PlantHealthLogs
{
    public class UpdatePlantHealthLogDto
    {
        [Required]
        [AllowedValues("Healthy", "Sick", "Yellow Leaves", "Brown Leaves", "Bugs")]
        public string Status { get; set; } = string.Empty;
    }
}