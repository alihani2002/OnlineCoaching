namespace OnlineCoaching.WebUI.Models.Clients
{
    public class ClientDashboardViewModel
    {
        public Client Client { get; set; } = null!;
        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
        public IEnumerable<ClientAnswer> Answers { get; set; } = new List<ClientAnswer>();
    }
}
