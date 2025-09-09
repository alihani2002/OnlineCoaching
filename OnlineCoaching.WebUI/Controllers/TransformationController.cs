using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.WebUI.Helper;

namespace OnlineCoaching.WebUI.Controllers
{
    public class TransformationController : Controller
    {
        private readonly ImageHelper _imageHelper;
        private readonly ITransformationService _transformationService;
        private readonly IClientService _clientService;
        public TransformationController(ImageHelper imageHelper, ITransformationService transformationService , IClientService clientService)
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

        public IActionResult BeforeAndAfter()
        {
            var transformations = _transformationService.GetAll();
            return View(transformations);
        }

        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Create()
        {
            var clients = _clientService.GetAllClients();
            ViewBag.Clients = clients ?? new List<Client>();

            var model = new Transformation(); 
            return View(model); 
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create(Transformation model, IFormFile? beforeImage, IFormFile? afterImage)
        {
            if (!ModelState.IsValid)
            {
                var clients = _clientService.GetAllClients();
                ViewBag.Clients = clients ?? new List<Client>();
                return View(model);
            }

            model.BeforeImageUrl = await _imageHelper.UploadImageAsync(beforeImage, "Transformations");
            model.AfterImageUrl = await _imageHelper.UploadImageAsync(afterImage, "Transformations");
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
        public async Task<IActionResult> Edit(int id, Transformation model, IFormFile? beforeImage, IFormFile? afterImage)
        {
            if (!ModelState.IsValid) return View(model);

            var existing = _transformationService.GetById(id);
            if (existing == null) return NotFound();

            existing.Titles = model.Titles;
            existing.Notes = model.Notes;
            existing.BeforeImageUrl = await _imageHelper.UpdateImageAsync(beforeImage, existing.BeforeImageUrl, "Transformations");
            existing.AfterImageUrl = await _imageHelper.UpdateImageAsync(afterImage, existing.AfterImageUrl, "Transformations");
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
    

