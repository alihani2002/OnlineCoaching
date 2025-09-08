namespace OnlineCoaching.Domain.Dtos
{
    public class CreateExerciseDto
    {
        [Required, MaxLength(150)]
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public int MuscleId { get; set; }
        public List<string> LinkUrls { get; set; } = new();
    }
}
