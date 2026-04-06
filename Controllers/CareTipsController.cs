using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;

namespace PlantPlanner.Controllers
{
    [Authorize]
    public class CareTipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CareTipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tips = await _context.CareTips
                .OrderBy(t => t.Category)
                .ThenBy(t => t.Title)
                .ToListAsync();

            return View(tips);
        }
    }
}