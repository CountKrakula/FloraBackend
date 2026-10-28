using System.Security.Claims;
using FloraBackend.DTOs.CareLogs;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/plants/{plantId:int}/carelogs")]
    [Authorize]
    public class CareLogsController : ControllerBase
    {
        private readonly ICareLogService _careLogService;

        public CareLogsController(ICareLogService careLogService)
        {
            _careLogService = careLogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CareLogDto>>> GetAll(int plantId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var careLogs = await _careLogService.GetAllByPlantAsync(
                plantId, userId.Value);

            if (careLogs == null) return NotFound();

            return Ok(careLogs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CareLogDto>> GetById(
            int plantId, int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var careLog = await _careLogService.GetByIdAsync(
                id, plantId, userId.Value);

            if (careLog == null) return NotFound();

            return Ok(careLog);
        }

        [HttpPost]
        public async Task<ActionResult<CareLogDto>> Create(
            int plantId, CreateCareLogDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var created = await _careLogService.CreateAsync(
                dto, plantId, userId.Value);

            if (created == null) return NotFound();

            return CreatedAtAction(
                nameof(GetById),
                new { plantId, id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int plantId, int id, UpdateCareLogDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _careLogService.UpdateAsync(
                id, dto, plantId, userId.Value);

            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int plantId, int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _careLogService.DeleteAsync(
                id, plantId, userId.Value);

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
