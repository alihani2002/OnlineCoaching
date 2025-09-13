namespace OnlineCoaching.Domain.Dtos.Cooking
{
    public class Cookingdto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "⚠️ Name is required")]
        [StringLength(50, ErrorMessage = "⚠️ Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️ Title is required")]
        [StringLength(150, ErrorMessage = "⚠️ Title cannot exceed 150 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️ Description is required")]
        [StringLength(500, ErrorMessage = "⚠️ Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "⚠️ Video URL is required")]
        [Url(ErrorMessage = "⚠️ Please enter a valid video URL")]
        public string VideoUrl { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public string? CreatedById { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
