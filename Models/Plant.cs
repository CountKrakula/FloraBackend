namespace FloraBackend.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Image> PlantImages { get; set; }
        public List<PlantHealthLog> PlantHealthLogs { get; set; }
    }
}
