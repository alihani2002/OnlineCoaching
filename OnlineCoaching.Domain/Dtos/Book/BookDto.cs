namespace OnlineCoaching.Domain.Dtos
{
    public class BookDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public int Price { get; set; }
        public string? FileUrl { get; set; }
        public bool HasDemo { get; set; }
        public string? DemoUrl { get; set; }
    }
}
