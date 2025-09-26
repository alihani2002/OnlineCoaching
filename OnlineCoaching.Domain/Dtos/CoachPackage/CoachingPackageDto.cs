namespace OnlineCoaching.Domain.Dtos
{
    public class CoachingPackageDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? VedioUrl { get; set; }
        public string? Description { get; set; }
        public int DurationInMonths { get; set; }
        public int Price { get; set; }

        public bool IsFreePlan { get; set; }

    }
}
