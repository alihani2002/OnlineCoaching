using OnlineCoaching.Domain.Dtos.AssignmentCoaching;

namespace OnlineCoaching.WebUI.Models.RequestPackage
{
    public class AssignedEditViewModel
    {
        public int ClientId { get; set; }
        public int RequestId { get; set; }
        public List<AssignExerciseDto> Exercises { get; set; } = new List<AssignExerciseDto>();
        public List<AssignFoodDto> Foods { get; set; } = new List<AssignFoodDto>();
    }
}
