namespace OnlineCoaching.Domain.Dtos.AssignmentCoaching
{
    public class AssignExerciseDto : BaseEntity
    {
        public int ExerciseId { get; set; }
        public string? NameOfExercise { get; set; }
        public string? ImageUrl { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
        public DayOfWeekEnum DayOfWeek { get; set; }
        public string? MuscleName { get; set; }
        // Relations
        public int ClientId { get; set; }
        public int CoachingPackageRequestId { get; set; }

    }
}
