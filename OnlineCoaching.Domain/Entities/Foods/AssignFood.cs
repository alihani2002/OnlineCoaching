using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Domain.Entities
{
    public class AssignFood : BaseEntity
    {
        public DateTime AssignedOn { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
        public int Quantity { get; set; } = 0;
        public int NumberOfServings { get; set; } = 0;
        public DayOfWeekEnum DayOfWeek { get; set; }
        public MealsNum MealNumber { get; set; }

        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public int FoodId { get; set; }
        public Food? Food { get; set; }

        public int? MealId { get; set; }
        public Meal? Meal { get; set; }

        public int? CoachingPackageId { get; set; }
        public CoachingPackage? coachingPackage { get; set; }


        public int CoachingPackageRequestId { get; set; }
        public CoachingPackageRequest? CoachingPackageRequest { get; set; }
    }

}
