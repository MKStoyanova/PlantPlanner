namespace PlantPlanner.ViewModels
{
    public class PlantListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Light { get; set; }
        public int WaterIntervalDays { get; set; }
        public string? Location { get; set; }

        public DateTime? LastWateredOn { get; set; }
        public string WateringMessage { get; set; } = string.Empty;

        public string? SoilName { get; set; }

    }
}
