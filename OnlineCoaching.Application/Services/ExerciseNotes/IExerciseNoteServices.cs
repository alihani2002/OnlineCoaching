using OnlineCoaching.Domain.Dtos.Notes;
namespace OnlineCoaching.Application.Services
{
    public interface IExerciseNoteServices
    {
        Task<ExerciseNoteDto> AddAsync(CreateExerciseNoteDto dto);
        Task<ExerciseNoteDto?> GetByIdAsync(int id);
        Task<IEnumerable<ExerciseNoteDto>> GetNotesByExerciseAsync(int exerciseId);
        Task<IEnumerable<ExerciseNoteDto>> GetAllWithDetailsAsync();
        Task<bool> DeleteAsync(int id);
    }
}
