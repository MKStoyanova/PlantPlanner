using System.ComponentModel.DataAnnotations;

namespace PlantPlanner.Models
{
    public class CareTip
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Category { get; set; } = null!; 

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = null!;
    }
}