namespace OnlineCoaching.WebUI.Helper
{
    public interface IVideoService
    {
        Task<(string Url, string PublicId)?> UploadVideoAsync(IFormFile file, string folder = "onlinecoaching/videos");
        Task<bool> DeleteVideoAsync(string publicId);
    }
}
