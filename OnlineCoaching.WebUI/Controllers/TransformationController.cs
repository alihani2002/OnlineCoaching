using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.WebUI.Helper;

namespace OnlineCoaching.WebUI.Controllers
{
    public class TransformationController : Controller
    {
        private readonly IImageService _imageHelper;
        private readonly ITransformationService _transformationService;
        private readonly IClientService _clientService;

        public TransformationController(IImageService imageHelper, ITransformationService transformationService, IClientService clientService)
        {
            _imageHelper = imageHelper;
            _transformationService = transformationService;
            _clientService = clientService;
        }

        public IActionResult Index()
        {
            var transformations = _transformationService.GetAll();
            return View(transformations);
        }

        public IActionResult ImageGallery()
        {
            var transformations = _transformationService.GetAll();
            return View(transformations);
        }

        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Create()
        {
            var clients = _clientService.GetAllClients();
            ViewBag.Clients = clients ?? new List<Client>();

            return View(new Transformation());
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create(Transformation model, IFormFile? ImageUrl)
        {
            if (!ModelState.IsValid)
            {
                var clients = _clientService.GetAllClients();
                ViewBag.Clients = clients ?? new List<Client>();
                return View(model);
            }

            if (ImageUrl != null)
                model.ImageUrl = await _imageHelper.UploadImageAsync(ImageUrl, "Transformations");

            model.CreatedOn = DateTime.Now;
            await _transformationService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Edit(int id)
        {
            var transformation = _transformationService.GetById(id);
            if (transformation == null) return NotFound();

            var clients = _clientService.GetAllClients();
            ViewBag.Clients = clients ?? new List<Client>();

            return View(transformation);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Edit(int id, Transformation model, IFormFile? ImageUrl)
        {
            if (!ModelState.IsValid)
            {
                var clients = _clientService.GetAllClients();
                ViewBag.Clients = clients ?? new List<Client>();
                return View(model);
            }

            var existing = _transformationService.GetById(id);
            if (existing == null) return NotFound();

            existing.Titles = model.Titles;
            existing.Notes = model.Notes;
            existing.ClientId = model.ClientId;

            if (ImageUrl != null)
                existing.ImageUrl = await _imageHelper.UploadImageAsync(ImageUrl, "Transformations");

            existing.LastUpdatedOn = DateTime.Now;

            _transformationService.Update(existing);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await _transformationService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
