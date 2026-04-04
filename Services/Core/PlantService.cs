using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;
using PlantPlanner.Services.Contracts;

namespace PlantPlanner.Services.Core
{
    public class PlantService : IPlantService
    {
        private readonly ApplicationDbContext _context;

        public PlantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plant>> GetAllAsync()
        {
            return await _context.Plants.ToListAsync();
        }
    }
}