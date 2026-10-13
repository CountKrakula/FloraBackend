namespace FloraBackend.Models
{
    public class Plant
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int PlantSpeciesId { get; set; }
        public PlantSpecies PlantSpecies { get; set; }
        public List<Image> PlantImages { get; set; }
        public List<PlantHealthLog> PlantHealthLogs { get; set; }
    }
}
