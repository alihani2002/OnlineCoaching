using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachingPackagesController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICoachingPackageServices _packageService;
        private readonly ICoachingPackageRequestService _requestServices;
        private readonly IImageService _imageService;
        private readonly IAssignmentService _assignmentService;

        public CoachingPackagesController(
            ICoachingPackageServices packageService,
            ICoachingPackageRequestService requestService,
            IClientService clientService,
            IImageService imageService,
            IAssignmentService assignmentService)
        {
            _packageService = packageService;
            _requestServices = requestService;
            _clientService = clientService;
            _imageService = imageService;
            _assignmentService = assignmentService;
        }

        public IActionResult GetFreePackage()
        {
            var freePack = _packageService.GetPackageFree();
            return View(freePack);
        }
        public IActionResult Index()
        {
            var packages = _packageService.GetCoachingPackages();
            return View(packages);
        }

        // CoachingPackagesController
        public async Task<IActionResult> GetCoachingPackage()
        {
            var packages = _packageService.GetCoachingPackages();

            var sessionId = User.GetUserId();
            if (string.IsNullOrEmpty(sessionId))
            {
                return View("GetCoachingPackage", packages); 
            }

            var client = await _clientService.GetClientAsync(sessionId);

            ViewBag.Client = client!.Id; 
            var existingRequest = _requestServices.GetActiveOrPendingRequest(client!.Id);
            if (existingRequest != null)
            {
                if (existingRequest.IsAnswerQuestion == false && existingRequest.Status == ClientStatus.Active)
                    return RedirectToAction("CompleteQuestion", "Questions", new { id = existingRequest.Id });
                else
                    return View("ClientRequest", existingRequest);
            }

            return View("GetCoachingPackage", packages);
        }

        [HttpGet]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> ClientRequest(int id)
        {
            var request = await _requestServices.GetRequestByIdAsync(id);
            if (request == null) return NotFound();
            return View(request);  
        }
  
        public async Task<IActionResult> Details(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();
            return View(package);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create(CreateCoachingPackageDto dto, IFormFile? ImageUrl)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                if (ImageUrl != null)
                    dto.ImageUrl = await _imageService.UploadImageAsync(ImageUrl, "CoachPackage") ?? string.Empty;
                

                else
                {
                    ModelState.AddModelError("", "Image is required.");
                    return View(dto);
                }
                dto.CreatedById = User.GetUserId();
                await _packageService.AddCoachingPackageAsync(dto);
                if (dto.IsFreePlan)
                    return RedirectToAction(nameof(GetFreePackage));
                else
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Edit(CoachingPackageDto dto, IFormFile? ImageUrl)
        {
            if (!ModelState.IsValid) return View(dto);

            var existing = await _packageService.GetCoachingPackageByIdAsync(dto.Id);
            if (existing == null) return NotFound();

            try
            {
                if (ImageUrl != null)
                {
                    dto.ImageUrl = await _imageService.UploadImageAsync(ImageUrl, "CoachPackage") ?? existing.ImageUrl;
                }
                else
                {
                    dto.ImageUrl = existing.ImageUrl;
                }

                var updated = await _packageService.UpdateCoachingPackageAsync(dto);
                if (updated == null)
                {
                    ModelState.AddModelError("", "Unable to update package.");
                    return View(dto);
                }

                if (dto.IsFreePlan)
                    return RedirectToAction(nameof(GetFreePackage));
                else
                    return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }


        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _packageService.DeleteCoachingPackageAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete package.");
            }

            return RedirectToAction("GetFreePackage", "CoachingPackages"); // Or your FreePackages view
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Coach)]
        public async Task<IActionResult> Restore(int id)
        {
            var result = await _packageService.RestoreCoachingPackageAsync(id);
            if (!result) return NotFound();

            return RedirectToAction("GetFreePackage", "CoachingPackages"); // Or your FreePackages view
        }
    }
}


