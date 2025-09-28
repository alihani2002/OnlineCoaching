using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.WebUI.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CoachingPackage> FreeCoachingPackages { get; set; } = new List<CoachingPackage>();
        public IEnumerable<CoachingPackageDto> coachingPackages { get; set; } = new List<CoachingPackageDto>();
        public Client Client { get; set; } = new Client();
    }
}
