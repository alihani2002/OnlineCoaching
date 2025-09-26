namespace OnlineCoaching.Domain.Entities
{
    public class CoachVideo : BaseEntity
    {
        public string? Title { get; set; }
        public string Url { get; set; } = null!;
        public string? Description { get; set; }
        // ✅ Needed for deletion/updating in Cloudinary
        public string? PublicId { get; set; } 
    }
}
