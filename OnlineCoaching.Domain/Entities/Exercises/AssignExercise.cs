namespace OnlineCoaching.Domain.Entities
{
    public class AssignExercise : BaseEntity
    {
        public string? NameOfExercise { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public string? Notes { get; set; }
        public List<int>? SelectedDays { get; set; } = new List<int>();

        public DayOfWeekEnum DayOfWeek { get; set; }


        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        public int CoachingPackageRequestId { get; set; }
        public CoachingPackageRequest? CoachingPackageRequest { get; set; }

        public int? CoachingPackageId { get; set; } 
        public CoachingPackage? coachingPackage { get; set; }

        public DateTime AssignedOn { get; set; } = DateTime.Now;
      
    }
}
