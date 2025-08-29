using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Enums;
using OnlineCoaching.WebUI.Models.RequestPackage;
namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachingPackageRequestsController : Controller
    {
        private readonly ICoachingPackageServices _coachingPackageServices;
        private readonly ICoachingPackageRequestService _requestService;
        private readonly IClientService _clientService;
        public CoachingPackageRequestsController(ICoachingPackageRequestService requestService, IClientService clientService , ICoachingPackageServices coachingPackage)
        {
            _requestService = requestService;
            _clientService = clientService;
            _coachingPackageServices = coachingPackage;
        }

        public IActionResult Index()
        {
            var requests = _requestService.GetRequests();
            return View(requests);
        }



        public async Task<IActionResult> Details(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }



        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var sessionId = User.GetUserId();
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Login", "Account"); 
            }

            var client = await _clientService.GetClientAsync(sessionId);
            if (client == null) return NotFound("Client not found.");

            var requests = _requestService.GetUserRequests(client.Id);
            return View(requests); 
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, ClientStatus status)
        {
            var updated = await _requestService.ManageRequestStatusAsync(id, status);
            if (updated == null)
            {
                return BadRequest(new { message = "Could not update request status." });
            }

            // Set a success message depending on the new status
            string message = status switch
            {
                ClientStatus.Pending => "Request set back to pending.",
                ClientStatus.Active => "Request approved successfully!",
                ClientStatus.Suspended => "Request suspended.",
                
                _ => "Status updated successfully."
            };

            TempData["Success"] = message;

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> EditStatus(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, ClientStatus status)
        {
            var updated = await _requestService.UpdateStatusAsync(id, status);
            if (updated == null) return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _requestService.DeleteRequestAsync(id);
            if (!deleted) return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ConfirmRequest(int packageId)
        {
            var sessionId = User.GetUserId();
            var client = await _clientService.GetClientAsync(sessionId); 
            if (client == null) return NotFound();

            var package = await _coachingPackageServices.GetCoachingPackageByIdAsync(packageId);
            if (package == null) return NotFound();

            var model = new ConfirmRequestViewModel
            {
                PackageId = package.Id,
                PackageTitle = package.Title,
                PackagePrice = package.Price,
                DurationInMonths = package.DurationInMonths,
                ClientId = client.Id , 
                CreatedById = sessionId 
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRequest(ConfirmRequestViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _requestService.CreateRequestAsync(model.PackageId, model.ClientId);

            return RedirectToAction("GetCoachingPackage", "CoachingPackages");
        }
    }
}
