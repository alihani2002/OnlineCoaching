using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.Gallery;

namespace OnlineCoaching.WebUI.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryServices _galleryServices;
        private readonly IImageService _imageHelper;

        public GalleryController(IGalleryServices galleryServices, IImageService imageHelper)
        {
            _galleryServices = galleryServices;
            _imageHelper = imageHelper;
        }


        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Index()
        {
            var images = _galleryServices.GetImages();
            return View(images);
        }

        public IActionResult Galleries()
        {
            var images = _galleryServices.GetImages();
            return View(images);
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGalleryDto dto, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                    dto.Image = await _imageHelper.UploadImageAsync(imageFile, "gallery");

                await _galleryServices.AddImage(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }


        [HttpGet]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult CreateImageProfile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImageProfile(CreateGalleryDto dto, IFormFile? coachImageFile)
        {
            if (ModelState.IsValid)
            {
                // Upload files
                if (coachImageFile != null)
                    dto.CoachImage = await _imageHelper.UploadImageAsync(coachImageFile, "profile");

                await _galleryServices.AddImage(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }


        // ================== UPDATE ==================

        [HttpGet]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Edit(int id)
        {
            var image = _galleryServices.GetImageById(id);
            if (image == null) return NotFound();
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GalleryDto dto, IFormFile? newImageFile)
        {
            if (ModelState.IsValid)
            {
                // If new file uploaded → replace old image
                if (newImageFile != null)
                    dto.Image = await _imageHelper.UpdateImageAsync(newImageFile, dto.Image, "gallery");

                _galleryServices.EditImage(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // ================== DELETE ==================

        [HttpGet]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var image = _galleryServices.GetImageById(id);
            if (image == null) return NotFound();
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _galleryServices.DeleteImage(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
