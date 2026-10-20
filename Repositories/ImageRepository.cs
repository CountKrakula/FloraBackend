using FloraBackend.Data;
using FloraBackend.Repositories.IRepository;
using FloraBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace FloraBackend.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private FloraDBContext _context;

        public ImageRepository(FloraDBContext context)
        {
            _context = context;
        }
        public async Task<Image> GetImage(int imgId)
        {
            Image image = await _context.Images.Where(i => i.ImageId == imgId).FirstOrDefaultAsync();
            return image;
        }

        //Goes through plant to get the correct User by Id and then checks if plantId isn't null
        public async Task<List<Image>> GetImageByUserId(int userId, int? plantId)
        {
            var query = _context.Images.Where(i => i.Plant.User.Id == userId);

            if (plantId != null)
            {
                query = query.Where(i => i.PlantId == plantId);
            }

            return await query.ToListAsync();
        }

        //Gets all the images to the specific plant
        public async Task<List<Image>> GetImageByPlantId(int plantId)
        {
            return await _context.Images.Where(i => i.PlantId == plantId).ToListAsync();
        }

        public async Task<int> CreateImage(Image image)
        {
            await _context.Images.AddAsync(image);
            var result = await _context.SaveChangesAsync();

            return result;
        }

        //Checks if the image already exists for that plant
        public async Task<bool> ImageExistsByHashAndPlant(string hash, int plantId)
        {
            return await _context.Images.AnyAsync(i => i.Hash == hash && i.PlantId == plantId);
        }

        //Changes profile picture from plantimage 1 to plantimage 2 as an example
        public async Task ClearProfilePicturesForPlant(int plantId)
        {
            await _context.Images
                .Where(i => i.PlantId == plantId && i.IsProfilePicture)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(i => i.IsProfilePicture, false));
        }
        // Update a single image
        public async Task<bool> UpdateImage(Image image)
        {
            _context.Images.Update(image);
            var result = await _context.SaveChangesAsync();
            if(result > 0)
            {
                return true;
            }
            return false;
        }

        // Delete by ID without fetching first
        public async Task<bool> DeleteImage(int imgId)
        {
            var result = await _context.Images
                .Where(i => i.ImageId == imgId)
                .ExecuteDeleteAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
    }
}
