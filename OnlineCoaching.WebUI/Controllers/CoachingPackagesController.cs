using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachingPackagesController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICoachingPackageServices _packageService;
        private readonly ICoachingPackageRequestService _requestServices;

        public CoachingPackagesController(ICoachingPackageServices packageServic , ICoachingPackageRequestService requestService ,IClientService clientService )
        {
            _packageService = packageServic;
            _requestServices = requestService;
            _clientService = clientService;
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
                if (existingRequest.IsAnswerQuestion == false && existingRequest.Status == Domain.Enums.ClientStatus.Active)
                {
                    return RedirectToAction("CompleteQuestion", "Clients", new { id = existingRequest.Id });
                }
                else
                {
                    return View("ClientRequest", existingRequest);
                }
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



        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var package = await _packageService.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package);
        }

        // GET: CoachingPackage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoachingPackage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Create(CreateCoachingPackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                await _packageService.AddCoachingPackageAsync(dto);
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
        public async Task<IActionResult> Edit(CoachingPackageDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _packageService.UpdateCoachingPackageAsync(dto);
            if (updated == null)
            {
                ModelState.AddModelError("", "Unable to update package.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
        }
    }
}
