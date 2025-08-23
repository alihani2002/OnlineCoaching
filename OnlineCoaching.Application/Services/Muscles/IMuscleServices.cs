namespace OnlineCoaching.Application
{
    public interface IMuscleServices
    {

            IEnumerable<MuscleDto> GetMuscles();
            Task<MuscleDto?> GetMuscleByIdAsync(int id);
            Task<MuscleDto> AddMuscleAsync(CreateMuscleDto muscleDto);
            Task<MuscleDto?> UpdateMuscleAsync(MuscleDto muscleDto);
            Task<bool> DeleteMuscleAsync(int id);
    }
}
