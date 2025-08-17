namespace OnlineCoaching.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        [Required, MaxLength(150)]
        public string? Name { get; set; }

        public string? Description { get; set; }
        public string? VideoUrl { get; set; }

        public int MuscleId { get; set; }
        public Muscle? Muscle { get; set; }

        public ICollection<AssignExercise>? AssignedExercises { get; set; }
    }
}
