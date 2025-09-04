using OnlineCoaching.Domain.Enums.DayOfWeek;

namespace OnlineCoaching.Domain.Dtos.AssignmentCoaching
{
    public class AssignFoodDto :BaseEntity
    {
        public int FoodId { get; set; }
        public string? FoodName { get; set; }

        public int Quantity { get; set; }
        public int NumberOfServings { get; set; } = 0;
        public string? Notes { get; set; }

        public DayOfWeekEnum DayOfWeek { get; set; }
        public MealsNum MealNumber { get; set; }

        public string DayOfWeekName { get; set; } = string.Empty;
        public string MealNumberName { get; set; } = string.Empty;
        // Relations
        public int ClientId { get; set; }
        public int? MealId { get; set; }
        public int CoachingPackageRequestId { get; set; }

    }
}
