using Microsoft.EntityFrameworkCore;
using PlantPlanner.Data;
using PlantPlanner.Models;
using PlantPlanner.Services.Contracts;
using PlantPlanner.ViewModels;

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

        public async Task DeleteAsync(int id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant != null)
            {
                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
        }

        public async Task WaterAsync(int id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null)
            {
                return;
            }

            var wateringLog = new WateringLog
            {
                PlantId = plant.Id,
                WateredOn = DateTime.UtcNow
            };

            _context.WateringLogs.Add(wateringLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plant>> GetFilteredAsync(string? searchTerm, int? soilId)
        {
            var query = _context.Plants
                .Include(p => p.Soil)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            if (soilId.HasValue)
            {
                query = query.Where(p => p.SoilId == soilId);
            }

            return await query
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlantListItemViewModel>> GetAllForIndexAsync(string? searchTerm, int? soilId)
        {
            var plants = (await GetFilteredAsync(searchTerm, soilId)).ToList();

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
                    SoilName = p.Soil != null ? p.Soil.Name : null,
                    LastWateredOn = last,
                    WateringMessage = message
                };
            });

            return result;
        }

        public async Task<(IEnumerable<PlantListItemViewModel> Plants, int TotalCount)> GetPagedForIndexAsync(string? searchTerm, int? soilId, int page, int pageSize)
        {
            var allPlants = (await GetAllForIndexAsync(searchTerm, soilId)).ToList();

            var totalCount = allPlants.Count;

            var pagedPlants = allPlants
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedPlants, totalCount);
        }
    }
}