using FloraBackend.DTOs.Profile;
using FloraBackend.Models;
using FloraBackend.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace FloraBackend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ProfileDto?> GetAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return null;
            }

            return new ProfileDto
            {
                Name = user.Name ?? string.Empty,
                Email = user.Email ?? string.Empty,
                IsEmailConfirmed = user.EmailConfirmed
            };
        }

        public async Task<bool> UpdateAsync(int userId, UpdateProfileDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            user.Name = dto.Name.Trim();
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
