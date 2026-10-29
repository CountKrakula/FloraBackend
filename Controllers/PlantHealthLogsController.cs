using System.Security.Claims;
using FloraBackend.DTOs.PlantHealthLogs;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantHealthLogsController : ControllerBase
    {
        private readonly IPlantHealthLogService _healthLogService;

        public PlantHealthLogsController(IPlantHealthLogService healthLogService)
        {
            _healthLogService = healthLogService;
        }

        [HttpGet("plant/{plantId}")]
        public async Task<ActionResult<List<PlantHealthLogDto>>> GetAllForPlant(int plantId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var logs = await _healthLogService.GetAllForPlantAsync(plantId, userId.Value);
            return Ok(logs);
        }

        [HttpPost]
        public async Task<ActionResult<PlantHealthLogDto>> Create(CreatePlantHealthLogDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var created = await _healthLogService.CreateAsync(dto, userId.Value);
            if (created == null) return NotFound("Plant not found");

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePlantHealthLogDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _healthLogService.UpdateAsync(id, dto, userId.Value);
            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _healthLogService.DeleteAsync(id, userId.Value);
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