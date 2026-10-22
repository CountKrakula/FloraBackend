using FloraBackend.DTOs.Images;
using FloraBackend.Models;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/plants/{plantId:int}/images")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ImageDTO>>> GetAllImages (int plantId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var images = await _imageService.GetUserImages(plantId, userId.Value);

            if(images == null || !images.Any())
            {
                return NoContent();
            }

            return Ok(images);
        }

        [HttpPost]
        public async Task<ActionResult<CreateImageDTO>> CreateImage([FromForm] int plantId, [FromForm] IFormFile file, [FromForm] bool isProfilePicture)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (file == null)
            {
                return BadRequest("Ingen fil skickades");
            }

            var createImage = await _imageService.CreateImage(file, userId.Value, plantId, isProfilePicture);
            

            return Created("", "Bild uppladdad");
        }

        [HttpPut("{imageId:int}")]
        public async Task<IActionResult> UpdateImage(int plantId, int imageId, [FromForm] IFormFile file)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (file == null)
            {
                return BadRequest("Ingen fil skickades");
            }

            var result = await _imageService.UpdateImage(imageId, file, userId.Value);
            if (!result.isSuccessful)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpDelete("{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int plantId, int imageId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _imageService.DeleteImage(imageId, userId.Value);
            if (!result.isSuccessful)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
    }
}
