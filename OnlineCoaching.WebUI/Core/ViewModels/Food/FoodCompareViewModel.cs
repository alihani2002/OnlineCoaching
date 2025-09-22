using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.WebUI.Core.ViewModels.Food
{
    public class FoodCompareViewModel
    {

        public int SelectedFoodId { get; set; }
        public int TargetFoodId { get; set; }  
        public int Grams { get; set; }

        public FoodDto? SelectedFood { get; set; }
        public FoodDto? TargetFood { get; set; }

        public double SelectedFoodCalories { get; set; }
        public double TargetFoodCalories { get; set; }

        public double RequiredTargetGrams { get; set; }

        // dropdowns
        public List<FoodDto> AllFoods { get; set; } = new();
    }
}

