namespace FloraBackend.Models
{
    public class PlantHealthLog
    {
        public int PlantHealthLogId { get; set; }
        public string Status { get; set; }
        public DateTime RecordedAt { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
