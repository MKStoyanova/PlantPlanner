using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.ViewModels;

namespace PlantPlanner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                PlantsCount = await _context.Plants.CountAsync(),
                RemindersCount = await _context.Reminders.CountAsync(),
                UsersCount = await _userManager.Users.CountAsync()
            };

            return View(model);
        }
    }
}