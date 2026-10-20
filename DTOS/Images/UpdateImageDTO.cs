namespace FloraBackend.DTOs.Images
{
    public class UpdateImageDTO
    {
        public string Url { get; set; }
        public string Hash { get; set; }
        public bool IsProfilePicture { get; set; }
    }
}
