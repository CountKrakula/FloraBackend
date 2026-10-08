using System.ComponentModel.DataAnnotations;

namespace FloraBackend.Models;

public class CareLog
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int PlantId { get; set; }
    public Plant? Plant { get; set; }
}