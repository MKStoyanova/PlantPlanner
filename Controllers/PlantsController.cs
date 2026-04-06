using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;
using PlantPlanner.ViewModels;
using PlantPlanner.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace PlantPlanner.Controllers
{
    [Authorize]
  
    public class PlantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlantService _plantService;

        public PlantsController(ApplicationDbContext context, IPlantService plantService)
        {
            _context = context;
            _plantService = plantService;
        }


        public async Task<IActionResult> Index(string? searchTerm, int? soilId)
        {
            var result = await _plantService.GetAllForIndexAsync(searchTerm, soilId);

            ViewBag.SoilId = new SelectList(
                _context.Soils.OrderBy(s => s.Name),
                "Id",
                "Name",
                soilId);

            ViewBag.SearchTerm = searchTerm;

            return View(result);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _plantService.GetByIdAsync(id.Value);

            if (plant == null)
            {
                return NotFound();
            }

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

            await _plantService.CreateAsync(plant);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plant = await _plantService.GetByIdAsync(id.Value);
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
                await _plantService.UpdateAsync(plant);
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
            await _plantService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Water(int id)
        {
            await _plantService.WaterAsync(id);

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
