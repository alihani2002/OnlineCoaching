namespace OnlineCoaching.Domain.Dtos
{
    public class CoachingPackageRequestDto : BaseEntity
    {
        public string? Titles { get; set; }
        public int Price { get; set; }
        public ClientStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsAnswerQuestion { get; set; }

        public int PackageId { get; set; }
        public string? PackageTitle { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }
}
