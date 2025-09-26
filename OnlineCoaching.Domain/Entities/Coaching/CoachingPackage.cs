namespace OnlineCoaching.Domain.Entities
{
    public class CoachingPackage : BaseEntity
    {
        [Required, MaxLength(150)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } 
        public string? VedioUrl { get; set; }
        public int DurationInMonths { get; set; }
        public int Price { get; set; } = 0;
        public bool IsFreePlan { get; set; } = false;

        public ICollection<AssignExercise>? AssignExercises { get; set; }
        public ICollection<AssignFood>? AssignFoods { get; set; }

    }
}
