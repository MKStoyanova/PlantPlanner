using PlantPlanner.Models;

namespace PlantPlanner.Services.Contracts
{
    public interface IPlantService
    {
        Task<IEnumerable<Plant>> GetAllAsync();
       
        Task<Plant?> GetByIdAsync(int id);

        Task UpdateAsync(Plant plant);

        Task DeleteAsync(int id);
    }
}