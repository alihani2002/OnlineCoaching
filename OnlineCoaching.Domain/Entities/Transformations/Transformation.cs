namespace OnlineCoaching.Domain.Entities
{
    public class Transformation : BaseEntity
    {
        public string? Titles { get; set; }
        public string? Notes { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
