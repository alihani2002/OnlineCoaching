namespace OnlineCoaching.Domain.Dtos
{
    public class CreateCoachingPackageDto 
    {
        [Required, MaxLength(150)]
        public string? Title { get; set; }

        public string? ImageUrl { get; set; } 
        public string? VedioUrl { get; set; }

        public string? Description { get; set; }

        [Range(1, 36, ErrorMessage = "Duration must be between 1 and 36 months.")]
        public int DurationInMonths { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public int Price { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
