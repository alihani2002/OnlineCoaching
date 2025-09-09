using OnlineCoaching.Domain.Dtos.AssignmentCoaching;

namespace OnlineCoaching.Application.Services
{
    public interface IAssignmentService
    {
        Task AssignExercisesAsync(int clientId, List<AssignExerciseDto> exercises);
        Task AssignFoodsAsync(int clientId, List<AssignFoodDto> foods);
        Task DeleteAssignedExerciseAsync(int assignmentId);
        Task DeleteAssignedFoodAsync(int assignmentId);

    }
}
