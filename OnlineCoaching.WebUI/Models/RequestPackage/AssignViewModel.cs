using OnlineCoaching.Domain.Dtos.AssignmentCoaching;

namespace OnlineCoaching.WebUI.Models.RequestPackage
{
    public class AssignViewModel
    {
        public int ClientId { get; set; }
        public int RequestId { get; set; }
        public List<AssignExerciseDto>? Exercises { get; set; } = new();
        public List<AssignFoodDto>? Foods { get; set; } = new();
        public List<Exercise> AvailableExercises { get; set; } = new();
        public List<Food> AvailableFoods { get; set; } = new();
    }
}
