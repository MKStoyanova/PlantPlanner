using System.ComponentModel.DataAnnotations;

namespace PlantPlanner.Models
{
    public class Soil
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = string.Empty;
    }
}
