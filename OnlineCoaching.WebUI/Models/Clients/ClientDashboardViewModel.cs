using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.Domain.Enums;
using OnlineCoaching.Domain.Enums.DayOfWeek;

namespace OnlineCoaching.WebUI.Models.Clients
{
    public class ClientDashboardViewModel
    {
        public Client Client { get; set; } = null!;
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
        public IEnumerable<ClientAnswer> Answers { get; set; } = new List<ClientAnswer>();


        // Add these
        public List<AssignExerciseDto> AssignedExercises { get; set; } = new();
        public List<AssignFoodDto> AssignedFoods { get; set; } = new();

        // Exercises grouped by DayOfWeek
        public Dictionary<DayOfWeekEnum, List<AssignExerciseDto>> ExercisesByDay { get; set; } = new();

        // Foods grouped by Meal
        public Dictionary<MealsNum, List<AssignFoodDto>> FoodsByMeal { get; set; } = new();
    }
}
