namespace OnlineCoaching.WebUI.Helper
{
   
        public class ImageHelper
        {
            private readonly IWebHostEnvironment _env;
            private readonly string _baseFolder;

            public ImageHelper(IWebHostEnvironment env, string baseFolder = "uploads")
            {
                _env = env;
                _baseFolder = baseFolder;
            }

            private async Task<string?> SaveFileAsync(IFormFile? file, string subFolder)
            {
                if (file == null) return null;

                var uploadsFolder = Path.Combine(_env.WebRootPath, _baseFolder, subFolder);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // return relative path (to use in <img src>)
                return $"/{_baseFolder}/{subFolder}/{fileName}";
            }

            public async Task<string?> UploadImageAsync(IFormFile? file, string subFolder = "")
            {
                return await SaveFileAsync(file, subFolder);
            }

            public async Task<string?> UpdateImageAsync(IFormFile? newFile, string? existingPath, string subFolder = "")
            {
                if (newFile == null) return existingPath;

                if (!string.IsNullOrEmpty(existingPath))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, existingPath.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                        File.Delete(oldFilePath);
                }

                return await SaveFileAsync(newFile, subFolder);
            }

            public string? GetImageUrl(string? path)
            {
                if (string.IsNullOrEmpty(path)) return null;

                var filePath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
                return File.Exists(filePath) ? path : null;
            }
        }
    }


