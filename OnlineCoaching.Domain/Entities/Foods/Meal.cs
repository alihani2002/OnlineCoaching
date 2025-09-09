namespace OnlineCoaching.Domain.Entities
{
    public class Meal : BaseEntity
    {
        public MealsNum MealNumber { get; set; }  
        public DayOfWeekEnum DayOfWeek { get; set; } 

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int CoachingPackageRequestId { get; set; }
        public CoachingPackageRequest? CoachingPackageRequest { get; set; }

        public ICollection<AssignFood>? Foods { get; set; }
    }
}
