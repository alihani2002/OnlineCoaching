namespace OnlineCoaching.Domain.Entities
{
    public class Cooking : BaseEntity
    {
        [Required, MaxLength(150)]
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? VideoUrl { get; set; }
    }
}
