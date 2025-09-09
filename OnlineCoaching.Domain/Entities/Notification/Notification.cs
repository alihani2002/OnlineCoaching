namespace OnlineCoaching.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string? Message { get; set; }
        public bool IsRead { get; set; } = false;
        public string? UserId { get; set; } 
        public ApplicationUser? User { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public new DateTime? CreatedOn { get; set; } = DateTime.Now;
        public NotificationType Type { get; set; }
    }
}