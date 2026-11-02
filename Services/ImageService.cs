using FloraBackend.DTOs.Images;
using FloraBackend.Models;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Services.IServices;
using System.Security.Cryptography;

namespace FloraBackend.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IPlantRepository _plantRepository;

        public ImageService(IImageRepository imageRepository, IPlantRepository plantRepository)
        {
            _imageRepository = imageRepository;
            _plantRepository = plantRepository;
        }

        public async Task<List<ImageDTO>> GetUserImages(int userId, int plantId)
        {
            var images = await _imageRepository.GetImageByUserId(userId, plantId);

            if (images == null || !images.Any())
            {
                return null;
            }

            List<ImageDTO> imageDTOList = new List<ImageDTO>();
            foreach (var img in images)
            {
                ImageDTO imageDTO = new ImageDTO
                {
                    ImageId = img.ImageId,
                    Url = img.Url,
                    IsProfilePicture = img.IsProfilePicture,
                    PlantId = img.PlantId
                };
                imageDTOList.Add(imageDTO);
            }
            return imageDTOList;
        }

        public static string? GetProfilePictureUrl(List<Image>? images)
        {
            if (images == null || !images.Any())
            {
                return null;
            }
            return images.FirstOrDefault(i => i.IsProfilePicture)?.Url
                ?? images.FirstOrDefault()?.Url;
        }

        public async Task<(bool isSuccesful, List<string?> Errors)> CreateImage(IFormFile file, int userId, int plantId, bool isProfilePicture)
        {
            // Checks if there is an image file or isn't too big
            if (file == null || file.Length == 0)
            {
                return (false, new List<string?> { "Ingen fil uppladdad" });
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return (false, new List<string?> { "Endast bilder är tillåtna" });
            }

            if (file.Length > 5 * 1024 * 1024)
            {
                return (false, new List<string?> { "Filen är för stor" });
            }

            if (plantId <= 0)
            {
                return (false, new List<string?> { "Ogiltigt PlantId" });
            }

            // Kollar direkt om växten finns och tillhör användaren
            var plant = await _plantRepository.GetByIdAsync(plantId, userId);
            if (plant == null)
            {
                return (false, new List<string?> { "Ogiltig plant" });
            }

            // Reads the files/images into memory first
            byte[] fileBytes;
            string hash;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Creates hash so images can't get duplicated
            using (var sha = SHA256.Create())
            {
                hash = Convert.ToBase64String(sha.ComputeHash(fileBytes));
            }

            // Checks for duplicates
            var imageExist = await _imageRepository.ImageExistsByHashAndPlant(hash, plantId);
            if (imageExist)
            {
                return (false, new List<string?> { "Denna bild finns redan uppladdad." });
            }

            // Saves it to wwwroot
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            // If profile picture is added or changed 
            if (isProfilePicture)
            {
                await _imageRepository.ClearProfilePicturesForPlant(plantId);
            }

            // Sends to database
            var image = new Image
            {
                Url = $"/uploads/{fileName}",
                PlantId = plantId,
                Hash = hash,
                IsProfilePicture = isProfilePicture
            };

            var result = await _imageRepository.CreateImage(image);

            if (result == 0)
            {
                return (false, new List<string?> { "Kunde inte spara bilden" });
            }

            return (true, new List<string?>());
        }

        public async Task<(bool isSuccessful, List<string?> Errors)> UpdateImage(int imageId, IFormFile newFile, int userId)
        {
            if (newFile == null || newFile.Length == 0)
            {
                return (false, new List<string?> { "Ingen fil uppladdad" });
            }

            if (!newFile.ContentType.StartsWith("image/"))
            {
                return (false, new List<string?> { "Endast bilder är tillåtna" });
            }

            if (newFile.Length > 5 * 1024 * 1024)
            {
                return (false, new List<string?> { "Filen är för stor" });
            }

            var existingImage = await _imageRepository.GetImage(imageId);
            if (existingImage == null)
            {
                return (false, new List<string?> { "Bilden hittades inte" });
            }

            var plant = await _plantRepository.GetByIdAsync(existingImage.PlantId, userId);
            if (plant?.UserId != userId)
            {
                return (false, new List<string?> { "Ogiltig åtkomst" });
            }

            byte[] fileBytes;
            string newHash;
            using (var memoryStream = new MemoryStream())
            {
                await newFile.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            using (var sha = SHA256.Create())
            {
                newHash = Convert.ToBase64String(sha.ComputeHash(fileBytes));
            }

            var oldFileName = Path.GetFileName(new Uri(existingImage.Url, UriKind.RelativeOrAbsolute).LocalPath);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", oldFileName);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);

            var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(newFile.FileName)}";
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadPath = Path.Combine(rootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var newFilePath = Path.Combine(uploadPath, newFileName);
            await File.WriteAllBytesAsync(newFilePath, fileBytes);

            existingImage.Url = $"/uploads/{newFileName}";
            existingImage.Hash = newHash;

            var result = await _imageRepository.UpdateImage(existingImage);

            return result
                ? (true, new List<string?>())
                : (false, new List<string?> { "Kunde inte uppdatera bilden" });
        }

        public async Task<(bool isSuccessful, List<string?> Errors)> DeleteImage(int imageId, int userId)
        {
            var image = await _imageRepository.GetImage(imageId);
            if (image == null)
            {
                return (false, new List<string?> { "Bilden hittades inte" });
            }

            var plant = await _plantRepository.GetByIdAsync(image.PlantId, userId);
            if (plant?.UserId != userId)
            {
                return (false, new List<string?> { "Ogiltig åtkomst" });
            }

            var fileName = Path.GetFileName(new Uri(image.Url, UriKind.RelativeOrAbsolute).LocalPath);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            var result = await _imageRepository.DeleteImage(imageId);

            return result
                ? (true, new List<string?>())
                : (false, new List<string?> { "Kunde inte ta bort bilden" });
        }
    }
}