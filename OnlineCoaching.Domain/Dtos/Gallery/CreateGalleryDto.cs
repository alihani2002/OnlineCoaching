namespace OnlineCoaching.Domain.Dtos.Gallery
{
    public class CreateGalleryDto
    {
        public string? CoachImage { get; set; }
        public string? Image { get; set; }
        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
