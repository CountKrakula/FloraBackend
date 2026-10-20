using System.Security.Claims;
using FloraBackend.DTOs.Plants;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantsController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantsController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlantDto>>> GetAll()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var plants = await _plantService.GetAllAsync(userId.Value);
            return Ok(plants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlantDto>> GetById(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var plant = await _plantService.GetByIdAsync(id, userId.Value);
            if (plant == null) return NotFound();

            return Ok(plant);
        }

        [HttpPost]
        public async Task<ActionResult<PlantDto>> Create(CreatePlantDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var created = await _plantService.CreateAsync(dto, userId.Value);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePlantDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _plantService.UpdateAsync(id, dto, userId.Value);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _plantService.DeleteAsync(id, userId.Value);
            if (!deleted) return NotFound();

            return NoContent();
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
    }
}