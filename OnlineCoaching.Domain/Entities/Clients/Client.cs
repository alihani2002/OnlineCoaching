namespace OnlineCoaching.Domain.Entities
{
    public class Client : BaseEntity
    {
        [Required, MaxLength(100)]
        public string? FullName { get; set; }
        public DateTime BirthDate { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<CoachingPackageRequest>? CoachingPackageRequests { get; set; }
        public virtual ICollection<BookRequest>? BookRequests { get; set; }
        public virtual ICollection<CourseRequest>? CourseEnrollments { get; set; }

        public virtual ICollection<AssignExercise>? AssignedExercises { get; set; }
        public virtual ICollection<AssignFood>? AssignedFoods { get; set; }
        public virtual ICollection<Meal>? Meals { get; set; }  

        public virtual ICollection<Transformation>? Transformations { get; set; }
        public virtual ICollection<CoachFeedback>? CoachFeedbacks { get; set; }

        public virtual ICollection<ClientAnswer> Answers { get; set; } = [];
        public virtual ICollection<ExerciseSheetLog> ExerciseSheetLogs { get; set; } = [];
    }

}
