namespace OnlineCoaching.Domain.Dtos
{
    public class ExerciseDto : BaseEntity
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public int MuscleId { get; set; }
        public string? MuscleName { get; set; }
        public List<string> LinkUrls { get; set; } = new();
    }
}
