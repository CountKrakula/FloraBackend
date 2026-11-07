using FloraBackend.DTOs.Profile;

namespace FloraBackend.Services.IServices
{
    public interface IProfileService
    {
        Task<ProfileDto?> GetAsync(int userId);
        Task<bool> UpdateAsync(int userId, UpdateProfileDto dto);
    }
}
