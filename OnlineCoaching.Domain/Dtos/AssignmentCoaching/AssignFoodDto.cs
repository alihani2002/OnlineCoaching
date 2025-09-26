namespace OnlineCoaching.Domain.Dtos.AssignmentCoaching
{
    public class AssignFoodDto :BaseEntity
    {
        public int FoodId { get; set; }

        [StringLength(100, ErrorMessage = "Food name cannot exceed 100 characters.")]
        public string? FoodName { get; set; }

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000 grams.")]
        public int Quantity { get; set; }

        [Range(1, 20, ErrorMessage = "Number of servings must be between 1 and 20.")]
        public int NumberOfServings { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        public DayOfWeekEnum DayOfWeek { get; set; }

        public MealsNum MealNumber { get; set; }

        public string DayOfWeekName { get; set; } = string.Empty;
        public string MealNumberName { get; set; } = string.Empty;
        // Relations
        public int? ClientId { get; set; }
        public int? MealId { get; set; }
        public int CoachingPackageRequestId { get; set; }

    }
}
