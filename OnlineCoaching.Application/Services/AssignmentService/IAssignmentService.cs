using OnlineCoaching.Domain.Dtos.AssignmentCoaching;

namespace OnlineCoaching.Application.Services
{
    public interface IAssignmentService
    {
        List<AssignExercise> GetAssignedExercise(int clientId, int requestId);
        List<AssignFood> GetAssignedFood(int clientId, int requestId);
        Task<List<AssignExercise>> GetAssignedExercisesAsync(int clientId);
        Task<List<AssignFood>> GetAssignedFoodsAsync(int clientId);
        CoachingPackageRequest GetAssignedPackageFree(int packageId);
        // ✅ Client assignments
        Task AssignExercisesAsync(int clientId, List<AssignExerciseDto> exercises);
        Task AssignFoodsAsync(int clientId, List<AssignFoodDto> foods);

        // ✅ Free package assignments
        Task AssignExercisesToFreePackageAsync(int packageId, List<AssignExerciseDto> exercises);
        Task AssignFoodsToFreePackageAsync(int packageId, List<AssignFoodDto> foods);

        // ✅ Deletions
        Task DeleteAssignedExerciseAsync(int assignmentId);
        Task DeleteAssignedFoodAsync(int assignmentId);
    }
}
