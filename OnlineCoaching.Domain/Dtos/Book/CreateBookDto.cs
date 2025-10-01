namespace OnlineCoaching.Domain.Dtos
{
    public class CreateBookDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public int Price { get; set; } = 0;
        public string? FileUrl { get; set; }
        public bool HasDemo { get; set; }
        public string? DemoUrl { get; set; }
    }
}
