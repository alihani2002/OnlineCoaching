namespace OnlineCoaching.Domain.Dtos
{
    public class ExerciseAlternativeDto : BaseEntity
    {
        public int ExerciseId { get; set; }
        public int AlternativeExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? AlternativeExerciseName { get; set; }
    }
}
