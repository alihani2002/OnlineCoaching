namespace OnlineCoaching.Domain.Dtos
{
    public class CreateCoachingPackageRequestDto 
    {
        public int PackageId { get; set; }
        public ClientStatus Status { get; set; }
        public string? ClientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAnswerQuestion { get; set; } = false;

        public string? CreatedById { get; set; } 
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
