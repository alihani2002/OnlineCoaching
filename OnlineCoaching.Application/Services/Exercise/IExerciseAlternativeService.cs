namespace OnlineCoaching.Application.Services
{
    public interface IExerciseAlternativeService
    {
        Task<ExerciseProfileDto?> GetExerciseProfileAsync(int exerciseId);
        Task<IEnumerable<Exercise>> GetAllExercisesWithAlternativesAsync();
        Task<IEnumerable<ExerciseAlternativeDto>> GetAlternativesByExerciseIdAsync(int exerciseId);
        Task<ExerciseAlternativeDto?> AddAlternativeAsync(CreateExerciseAlternativeDto dto);
        Task<bool> DeleteAlternativeAsync(int exerciseId, int alternativeExerciseId);
    }
}
