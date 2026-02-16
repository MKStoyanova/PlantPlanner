using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace PlantPlanner.Models
{
    public class Plant
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Name { get; set; } = null!;

        [StringLength(60)]
        public string? Type { get; set; }

        [StringLength(20)]
        public string? Light { get; set; }

        [Display(Name = "Days until it needs water")]
        [Range(1, 365, ErrorMessage = "Please enter a value between 1 and 365 days.")]
        public int WaterIntervalDays { get; set; } = 7;

        public int? SoilId { get; set; }
        public Soil? Soil { get; set; }


        [StringLength(60)]
        public string? Location { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
