namespace OnlineCoaching.Domain.Entities
{
    public class Exercise : BaseEntity
    {
        [Required, MaxLength(150)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? VideoUrl { get; set; }
        public List<string> LinkUrls { get; set; } = [];

        public int MuscleId { get; set; }
        public Muscle? Muscle { get; set; }

        public ICollection<AssignExercise>? AssignedExercises { get; set; }
        public ICollection<ExerciseSheetLog>? ExerciseLogs { get; set; }

        public ICollection<ExerciseAlternative> Alternatives { get; set; } = [];   // التمارين اللي ليها بدائل
        public ICollection<ExerciseAlternative> AlternativeTo { get; set; } = []; // التمارين اللي أنا بديل ليها


    }
}
