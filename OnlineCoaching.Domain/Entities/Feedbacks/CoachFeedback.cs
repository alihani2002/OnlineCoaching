namespace OnlineCoaching.Domain.Entities
{
    public class CoachFeedback : BaseEntity
    {
        public string? FeedbackText { get; set; }
        public int Rating { get; set; } // 1 to 5 stars

        public int ClientId { get; set; }
        public Client? Client { get; set; }

    }
}
