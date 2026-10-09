namespace FloraBackend.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public bool IsProfilePicture { get; set; }

        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
