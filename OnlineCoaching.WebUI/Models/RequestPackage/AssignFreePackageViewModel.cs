using OnlineCoaching.Domain.Dtos.AssignmentCoaching;

namespace OnlineCoaching.WebUI.Models.RequestPackage
{
    public class AssignFreePackageViewModel
    {
        public int PackageId { get; set; }

        // Exercises
        public List<AssignExerciseDto> Exercises { get; set; } = new List<AssignExerciseDto>();
        public List<Exercise> AvailableExercises { get; set; } = new List<Exercise>();

        // Foods
        public List<AssignFoodDto> Foods { get; set; } = new List<AssignFoodDto>();
        public List<Food> AvailableFoods { get; set; } = new List<Food>();
    }
}
