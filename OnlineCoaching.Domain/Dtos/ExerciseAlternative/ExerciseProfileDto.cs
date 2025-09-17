namespace OnlineCoaching.Domain.Dtos
{
    public class ExerciseProfileDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? VideoUrl { get; set; }

        public List<ExerciseAlternativeDto> Alternatives { get; set; } = new();
    }
}

