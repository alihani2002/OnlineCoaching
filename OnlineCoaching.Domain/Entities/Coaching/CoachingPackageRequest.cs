namespace OnlineCoaching.Domain.Entities
{
    public class CoachingPackageRequest : BaseEntity
    {
        public string? Titles { get; set; }
        public int Price { get; set; } = 0;
        public ClientStatus Status { get; set; } = ClientStatus.Pending;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int PackageId { get; set; }
        public CoachingPackage? Package { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }

}
