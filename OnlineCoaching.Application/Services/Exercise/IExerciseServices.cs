namespace OnlineCoaching.Application.Services
{
    public interface IExerciseServices
    {
            IEnumerable<ExerciseDto> GetExercises();
            Task<ExerciseDto?> GetExerciseByIdAsync(int id);
            Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto dto);
            Task<ExerciseDto?> UpdateExerciseAsync(ExerciseDto dto);
            Task<bool> DeleteExerciseAsync(int id);
        }
    }

