namespace OnlineCoaching.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } 
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}