namespace OnlineCoaching.Domain.Dtos
{
    public class CreateMuscleDto
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; } = null;
    }
}
