using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.Cooking;
using OnlineCoaching.WebUI.Helper;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CookingController : Controller
    {
        private readonly ICookingServices _cookingServices;
        private readonly ImageHelper _imageHelper;

        public CookingController(ICookingServices cookingServices, ImageHelper imageHelper)
        {
            _cookingServices = cookingServices;
            _imageHelper = imageHelper;
        }

        public IActionResult Index()
        {
            var cookings = _cookingServices.GetCookings();
            return View(cookings);
        }

        public IActionResult CookingDesh()
        {
            var cookings = _cookingServices.GetCookings();
            return View(cookings);
        }

        public IActionResult Details(int id)
        {
            var cooking = _cookingServices.GetCookingById(id);
            if (cooking == null)
                return NotFound();

            return View(cooking);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cookingdto dto, IFormFile? imageFile)
        {           

            if (!ModelState.IsValid)
                return View(dto);

            // Upload image and set path
            dto.ImageUrl = await _imageHelper.UploadImageAsync(imageFile, "cookings");

            await _cookingServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var cooking = _cookingServices.GetCookingById(id);
            if (cooking == null)
                return NotFound();

            return View(cooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cookingdto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var existing = _cookingServices.GetCookingById(dto.Id);
            if (existing == null)
                return NotFound();
            if (imageFile == null)
            {
                dto.ImageUrl = existing.ImageUrl ;
            }
            else
            {
                dto.ImageUrl = await _imageHelper.UpdateImageAsync(imageFile, existing.ImageUrl, "cookings");
            }
            var updated = await _cookingServices.Update(dto.Id, dto);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cooking = _cookingServices.GetCookingById(id);
            if (cooking == null)
                return NotFound();

            return View(cooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _cookingServices.DeleteCookingAsync(id);

            if (!deleted)
            {
                ModelState.AddModelError("", "⚠️ Cooking not found or already deleted.");
                return RedirectToAction(nameof(Index) );
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
