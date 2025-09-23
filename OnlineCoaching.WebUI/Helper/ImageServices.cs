using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace OnlineCoaching.WebUI.Helper
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB in bytes

        public ImageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadImageAsync(IFormFile file, string folder = "onlinecoaching")
        {
            if (file == null || file.Length == 0)
                return null;

            // ✅ Validate size
            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("Image size cannot exceed 5 MB.");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl?.ToString();
        }

        public async Task<string?> UpdateImageAsync(IFormFile newFile, string? existingUrl, string folder = "onlinecoaching")
        {
            if (newFile == null) return existingUrl;

            return await UploadImageAsync(newFile, folder);
        }
    }
}
