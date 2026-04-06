using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;
using System.Security.Claims;

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
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var reminders = await _context.Reminders
                .Include(r => r.Plant)
                .Where(r => r.OwnerId == ownerId)
                .OrderBy(r => r.Date)
                .ToListAsync();

            return View(reminders);
        }

        public IActionResult Create()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ViewBag.PlantId = new SelectList(
                _context.Plants
                    .Where(p => p.OwnerId == ownerId)
                    .OrderBy(p => p.Name),
                "Id",
                "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reminder reminder)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("Plant");
            ModelState.Remove("OwnerId");

            if (!ModelState.IsValid)
            {
                ViewBag.PlantId = new SelectList(
                    _context.Plants
                        .Where(p => p.OwnerId == ownerId)
                        .OrderBy(p => p.Name),
                    "Id",
                    "Name",
                    reminder.PlantId);

                return View(reminder);
            }

            reminder.OwnerId = ownerId;

            var plantBelongsToUser = await _context.Plants
            .AnyAsync(p => p.Id == reminder.PlantId && p.OwnerId == ownerId);

            if (!plantBelongsToUser)
            {
                return Forbid();
            }

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}