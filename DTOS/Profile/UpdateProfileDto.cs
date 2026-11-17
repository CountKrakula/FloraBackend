using System.ComponentModel.DataAnnotations;

namespace FloraBackend.DTOs.Profile
{
    public class UpdateProfileDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
