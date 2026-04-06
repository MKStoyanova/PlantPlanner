using System.ComponentModel.DataAnnotations;

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

        public int PlantId { get; set; }

        public Plant Plant { get; set; } = null!;
    }
}