using FloraBackend.DTOs.Profile;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FloraBackend.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<ActionResult<ProfileDto>> Get()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var profile = await _profileService.GetAsync(userId.Value);
            if (profile == null) return NotFound();

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProfileDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _profileService.UpdateAsync(userId.Value, dto);
            if (!updated) return BadRequest();

            return NoContent();
        }

        private int? GetUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(value, out var id) ? id : null;
        }
    }
}
