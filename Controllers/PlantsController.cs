using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;

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
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(plants);
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id);
            if (plant == null) return NotFound();

            return View(plant);
        }

      
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (!ModelState.IsValid) return View(plant);

            _context.Add(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            return View(plant);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Plant plant)
        {
            if (id != plant.Id) return NotFound();
            if (!ModelState.IsValid) return View(plant);

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

            var plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id);
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
    }
}
