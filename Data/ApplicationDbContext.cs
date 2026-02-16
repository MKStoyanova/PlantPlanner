using Microsoft.EntityFrameworkCore;
using PlantPlanner.Models;

namespace PlantPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Plant> Plants { get; set; } = null!;

        public DbSet<WateringLog> WateringLogs { get; set; } = null!;

        public DbSet<Soil> Soils { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Soil>().HasData(
                new Soil { Id = 1, Name = "Standard" },
                new Soil { Id = 2, Name = "For carnivorous plants" },
                new Soil { Id = 3, Name = "For orchids" },
                new Soil { Id = 4, Name = "For cacti/succulents" },
                new Soil { Id = 5, Name = "For flowering" }
            );
        }


    }
}
