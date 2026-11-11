using FloraBackend.DTOs.CareTasks;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/plants/{plantId:int}/caretasks")]
    [Authorize]
    public class CareTasksController : ControllerBase
    {
        private readonly ICareTaskService _careTaskService;

        public CareTasksController(ICareTaskService careTaskService)
        {
            _careTaskService = careTaskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CareTaskDto>>> GetAll(int plantId)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var tasks = await _careTaskService.GetAllByPlantAsync(
                plantId, userId.Value);

            return Ok(tasks);
        }

        [HttpGet("~/api/caretasks")]
        public async Task<ActionResult<List<CareTaskDto>>> GetAllForUser()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var tasks = await _careTaskService.GetAllByUserAsync(userId);

            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CareTaskDto>> GetById(int plantId, int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = await _careTaskService.GetByIdAsync(
                id, plantId, userId.Value);

            if (task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<CareTaskDto>> Create(
            int plantId, CreateCareTaskDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var created = await _careTaskService.CreateAsync(
                dto, plantId, userId.Value);

            if (created == null) return NotFound();

            return CreatedAtAction(
                nameof(GetById),
                new { plantId, id = created.Id },
                created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int plantId, int id, UpdateCareTaskDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _careTaskService.UpdateAsync(
                id, dto, plantId, userId.Value);

            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int plantId, int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _careTaskService.DeleteAsync(
                id, plantId, userId.Value);

            if (!deleted) return NotFound();

            return NoContent();
        }

        [HttpPost("{id:int}/complete")]
        public async Task<ActionResult<CareTaskDto>> Complete(int plantId, int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var task = await _careTaskService.CompleteAsync(
                id, plantId, userId.Value);

            if (task == null) return NotFound();

            return Ok(task);
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }

    }
}
