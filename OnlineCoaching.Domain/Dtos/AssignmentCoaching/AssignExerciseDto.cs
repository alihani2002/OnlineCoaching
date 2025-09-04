using OnlineCoaching.Domain.Enums.DayOfWeek;

namespace OnlineCoaching.Domain.Dtos.AssignmentCoaching
{
    public class AssignExerciseDto : BaseEntity
    {
        public int ExerciseId { get; set; }
        public string? NameOfExercise { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
        public DayOfWeekEnum DayOfWeek { get; set; }

        // Relations
        public int ClientId { get; set; }
        public int CoachingPackageRequestId { get; set; }

    }
}
