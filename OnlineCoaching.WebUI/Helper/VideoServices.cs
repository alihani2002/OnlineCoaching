using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace OnlineCoaching.WebUI.Helper
{
    public class VideoService : IVideoService
    {
        private readonly Cloudinary _cloudinary;
        private const long MaxVideoSize = 50 * 1024 * 1024; // 50 MB in bytes (adjust as needed)

        public VideoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        /// <summary>
        /// Uploads a video to Cloudinary
        /// </summary>
        public async Task<(string Url, string PublicId)?> UploadVideoAsync(IFormFile file, string folder = "onlinecoaching/videos")
        {
            if (file == null || file.Length == 0)
                return null;

            // ✅ Validate size
            if (file.Length > MaxVideoSize)
                throw new InvalidOperationException("Video size cannot exceed 50 MB.");

            await using var stream = file.OpenReadStream();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return (result.SecureUrl?.ToString()!, result.PublicId);
        }

        /// <summary>
        /// Deletes a video from Cloudinary by its publicId
        /// </summary>
        public async Task<bool> DeleteVideoAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return false;

            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video
            };

            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }
    }
}
