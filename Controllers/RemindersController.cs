using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;

namespace PlantPlanner.Controllers
{
    [Authorize]
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RemindersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reminders = await _context.Reminders
                .Include(r => r.Plant)
                .OrderBy(r => r.Date)
                .ToListAsync();

            return View(reminders);
        }

        public IActionResult Create()
        {
            ViewBag.PlantId = new SelectList(_context.Plants.OrderBy(p => p.Name), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reminder reminder)
        {
           
            ModelState.Remove("Plant");

            if (ModelState.IsValid)
            {
                _context.Add(reminder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PlantId = new SelectList(_context.Plants, "Id", "Name", reminder.PlantId);
            return View(reminder);
        }
    }
}