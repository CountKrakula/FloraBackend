using System.ComponentModel.DataAnnotations;

namespace FloraBackend.Models;

public class CareTask
{
    public int Id { get; set; }

    public CareTaskType Type { get; set; }

    [Range(1, 365)]
    public int IntervalDays { get; set; }

    public DateTime? LastDoneAt { get; set; }

    public DateTime NextDueAt { get; set; }

    public int PlantId { get; set; }
    public Plant? Plant { get; set; }
}

public enum CareTaskType
{
    Watering,
    Fertilize,
    Prune,
    Repot
}