using FloraBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FloraBackend.Data
{
    public class FloraDBContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FloraDBContext(DbContextOptions<FloraDBContext> options) : base(options)
        {

        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantSpecies> PlantSpecies { get; set; }
        public DbSet<PlantHealthLog> PlantHealthLogs { get; set; }
        public DbSet<CareLog> CareLogs { get; set; }
        public DbSet<CareTask> CareTasks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
