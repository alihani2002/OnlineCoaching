namespace OnlineCoaching.Application.Services
{
    public interface ICoachingPackageServices
    {
        IEnumerable<CoachingPackageDto> GetCoachingPackages();
        Task<CoachingPackageDto?> GetCoachingPackageByIdAsync(int id);
        Task<CoachingPackageDto> AddCoachingPackageAsync(CreateCoachingPackageDto dto);
        Task<CoachingPackageDto?> UpdateCoachingPackageAsync(CoachingPackageDto dto);
        Task<bool> DeleteCoachingPackageAsync(int id);
    }
}
