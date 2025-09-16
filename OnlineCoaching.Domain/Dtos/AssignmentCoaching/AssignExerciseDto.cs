namespace OnlineCoaching.Domain.Dtos.AssignmentCoaching
{
    public class AssignExerciseDto : BaseEntity
    {
            public int ExerciseId { get; set; }

            [StringLength(100, ErrorMessage = "Exercise name cannot exceed 100 characters.")]
            public string? NameOfExercise { get; set; }

            [Url(ErrorMessage = "Invalid video URL format.")]
            public string? VideoUrl { get; set; }

            [StringLength(200, ErrorMessage = "Image URL cannot exceed 200 characters.")]
            public string? ImageUrl { get; set; }

            [Range(1, 20, ErrorMessage = "Sets must be between 1 and 20.")]
            public int Sets { get; set; }

            [Range(1, 100, ErrorMessage = "Reps must be between 1 and 100.")]
            public int Reps { get; set; }

            [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
            public string? Notes { get; set; }

            public DayOfWeekEnum DayOfWeek { get; set; }

            [StringLength(100, ErrorMessage = "Muscle name cannot exceed 100 characters.")]
            public string? MuscleName { get; set; }

            public int ClientId { get; set; }
            public int CoachingPackageRequestId { get; set; }
        }

    }
