namespace OnlineCoaching.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? InstructorName { get; set; }
        public int Price { get; set; } = 0;
        public int NumberOfVideos { get; set; } = 0;
        public bool HasDemo { get; set; } = false;
    }
}
