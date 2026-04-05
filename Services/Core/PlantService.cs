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
            return await _context.Plants
                .Include(p => p.Soil)
                .ToListAsync();
        }

        public async Task<Plant?> GetByIdAsync(int id)
        {
            return await _context.Plants
                .Include(p => p.Soil)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(Plant plant)
        {
            _context.Update(plant);
            await _context.SaveChangesAsync();
        }
    }
}