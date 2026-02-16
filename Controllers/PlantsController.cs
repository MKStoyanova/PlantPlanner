using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;
using PlantPlanner.ViewModels;

namespace PlantPlanner.Controllers
{
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            
            var plants = await _context.Plants
                .Include(p => p.Soil)
                .OrderBy(p => p.Name)
                .ToListAsync();

            
            var lastWaterings = await _context.WateringLogs
                .GroupBy(w => w.PlantId)
                .Select(g => new
                {
                    PlantId = g.Key,
                    LastWateredOn = g.Max(x => x.WateredOn)
                })
                .ToListAsync();

            var lastByPlantId = lastWaterings
                .ToDictionary(x => x.PlantId, x => (DateTime?)x.LastWateredOn);

            var today = DateTime.UtcNow.Date;

            
            var result = plants.Select(p =>
            {
                lastByPlantId.TryGetValue(p.Id, out var last);

                string message;
                if (last == null)
                {
                    message = "No watering yet.";
                }
                else
                {
                    var lastDate = last.Value.Date;
                    var daysSince = (today - lastDate).Days;

                    var nextWaterDate = lastDate.AddDays(p.WaterIntervalDays);
                    var daysUntil = (nextWaterDate - today).Days;

                    if (daysUntil <= 0)
                    {
                        message = $"Don't forget to water today! It's been {daysSince} days since last watering.";
                    }
                    else
                    {
                        message = $"It will need water in {daysUntil} days.";
                    }
                }

                return new PlantListItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    Light = p.Light,
                    WaterIntervalDays = p.WaterIntervalDays,
                    Location = p.Location,
                    LastWateredOn = last,
                    WateringMessage = message,

                    
                    
                    SoilName = p.Soil != null ? p.Soil.Name : null
                };
            }).ToList();

            return View(result);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants
                .Include(p => p.Soil)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant == null) return NotFound();

            return View(plant);
        }

        
        public IActionResult Create()
        {
            PopulateSoilsDropDownList();
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            
            if (plant.SoilId == null)
            {
                ModelState.AddModelError(nameof(Plant.SoilId), "Please select soil.");
            }

            if (!ModelState.IsValid)
            {
                PopulateSoilsDropDownList(plant.SoilId);
                return View(plant);
            }

            _context.Add(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            PopulateSoilsDropDownList(plant.SoilId);
            return View(plant);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plant plant)
        {
            if (id != plant.Id) return NotFound();

            if (plant.SoilId == null)
            {
                ModelState.AddModelError(nameof(Plant.SoilId), "Please select soil.");
            }

            if (!ModelState.IsValid)
            {
                PopulateSoilsDropDownList(plant.SoilId);
                return View(plant);
            }

            try
            {
                _context.Update(plant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _context.Plants.AnyAsync(p => p.Id == id);
                if (!exists) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants
                .Include(p => p.Soil)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant == null) return NotFound();

            return View(plant);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant != null)
            {
                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Water(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            var wateringLog = new WateringLog
            {
                PlantId = plant.Id,
                WateredOn = DateTime.UtcNow
            };

            _context.WateringLogs.Add(wateringLog);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Plant successfully watered!";
            return RedirectToAction(nameof(Index));
        }

        
        private void PopulateSoilsDropDownList(int? selectedSoilId = null)
        {
            ViewBag.SoilId = new SelectList(
                _context.Soils.OrderBy(s => s.Name),
                "Id",
                "Name",
                selectedSoilId
            );
        }
    }
}
