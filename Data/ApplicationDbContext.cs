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
    }
}
