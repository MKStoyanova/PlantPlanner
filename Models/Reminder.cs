using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PlantPlanner.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Message { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a plant.")]
        public int PlantId { get; set; }

        [ValidateNever]
        public Plant? Plant { get; set; }
    }
}