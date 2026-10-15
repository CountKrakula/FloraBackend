using Microsoft.AspNetCore.Identity;

namespace FloraBackend.Models
{
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public int SeedPos { get; set; } = 0;
    }
}
