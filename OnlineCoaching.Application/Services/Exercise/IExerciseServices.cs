using OnlineCoaching.Domain.Dtos.ExerciseSheet;

namespace OnlineCoaching.Application.Services
{
    public interface IExerciseServices
    {
            IEnumerable<ExerciseDto> GetExercises();
            Task<ExerciseDto?> GetExerciseByIdAsync(int id);
            Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto dto);
            Task<ExerciseDto?> UpdateExerciseAsync(ExerciseDto dto);
            Task<bool> DeleteExerciseAsync(int id);


            Task<IEnumerable<ExerciseSheetLog>> GetAllLogs();
            Task<IEnumerable<ExerciseSheetLog>> GetLogsByClientAndExercise(int clientId, int exerciseId);
            Task<ExerciseSheetLog> AddExerciseSheetLog(CreateExerciseSheetDto dto);
            Task<ExerciseSheetLog?> UpdateExerciseSheetLog(ExerciseSheetLog dto);
            Task<bool> DeletedLogs(int id);
    }
}

