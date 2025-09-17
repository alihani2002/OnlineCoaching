namespace OnlineCoaching.Domain.Entities
{
    public class ExerciseAlternative
    {
        public int ExerciseId { get; set; }    
        public Exercise Exercise { get; set; } = null!;

        public int AlternativeExerciseId { get; set; } 
        public Exercise AlternativeExercise { get; set; } = null!;
    }
}
