namespace OnlineCoaching.WebUI.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<CoachingPackage> CoachingPackages { get; set; } = new List<CoachingPackage>();
        public Client Client { get; set; } = new Client();
    }
}
