using PlantPlanner.Models;
using PlantPlanner.ViewModels;

namespace PlantPlanner.Services.Contracts
{
    public interface IPlantService
    {
        Task<IEnumerable<Plant>> GetAllAsync();
       
        Task<Plant?> GetByIdAsync(int id);

        Task UpdateAsync(Plant plant);

        Task DeleteAsync(int id);

        Task CreateAsync(Plant plant);

        Task WaterAsync(int id);

        Task<IEnumerable<Plant>> GetFilteredAsync(string? searchTerm, int? soilId);
       
        Task<IEnumerable<PlantListItemViewModel>> GetAllForIndexAsync(string? searchTerm, int? soilId);

        Task<(IEnumerable<PlantListItemViewModel> Plants, int TotalCount)> GetPagedForIndexAsync(string? searchTerm, int? soilId, int page, int pageSize);
    }
}