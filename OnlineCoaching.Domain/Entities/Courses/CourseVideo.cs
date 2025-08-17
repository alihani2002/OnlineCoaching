namespace OnlineCoaching.Domain.Entities
{
    public class CourseVideo : BaseEntity
    {
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
