namespace FloraBackend.DTOs.Images
{
    public class ImageDTO
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
        public string Hash { get; set; }
        public bool IsProfilePicture { get; set; }

        public int PlantId { get; set; }
    }
}
