namespace OnlineCoaching.WebUI
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile file, string folder = "onlinecoaching");
        Task<string?> UpdateImageAsync(IFormFile newFile, string? existingUrl, string folder = "onlinecoaching");
        Task<string?> UploadPdfAsync(IFormFile file, string folder = "onlinecoaching/books");
    }
}

