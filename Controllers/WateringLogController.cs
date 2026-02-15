using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;

namespace PlantPlanner.Controllers
{
    public class WateringLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WateringLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int plantId)
        {
            var plant = await _context.Plants
                .FirstOrDefaultAsync(p => p.Id == plantId);

            if (plant == null)
            {
                return NotFound();
            }

            var logs = await _context.WateringLogs
                .Where(w => w.PlantId == plantId)
                .OrderByDescending(w => w.WateredOn)
                .ToListAsync();

            ViewBag.PlantName = plant.Name;

            return View(logs);
        }

    }
}
