using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantPlanner.Models
{
    public class WateringLog
    {
        public int Id { get; set; }

        [Required]
        public int PlantId { get; set; }

        [ForeignKey(nameof(PlantId))]
        public Plant Plant { get; set; } = null!;

        public DateTime WateredOn { get; set; } = DateTime.UtcNow;
    }
}
