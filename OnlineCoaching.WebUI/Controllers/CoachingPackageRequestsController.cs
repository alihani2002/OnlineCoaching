using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.WebUI.Models.RequestPackage;
namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize]
    public class CoachingPackageRequestsController(ICoachingPackageRequestService requestService, IClientService clientService, ICoachingPackageServices coachingPackage) : Controller
    {
        private readonly ICoachingPackageServices _coachingPackageServices = coachingPackage;
        private readonly ICoachingPackageRequestService _requestService = requestService;
        private readonly IClientService _clientService = clientService;


        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Index()
        {
            var requests = _requestService.GetRequests();
            return View(requests);
        }


        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }



      
        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
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


        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> EditStatus(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> EditStatus(int id, ClientStatus status)
        {
            var updated = await _requestService.UpdateStatusAsync(id, status);
            if (updated == null) return BadRequest();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _requestService.DeleteRequestAsync(id);
            if (!deleted) return BadRequest();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = AppRoles.Client)]
        [HttpGet]
        public async Task<IActionResult> ConfirmRequestView(int id)
        {
            var package = await _coachingPackageServices.GetCoachingPackageByIdAsync(id);
            if (package == null) return NotFound();

            return View(package); 
        }

        [Authorize(Roles = AppRoles.Client)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRequest(int packageId)
        {
            var sessionId = User.GetUserId();
            var client = await _clientService.GetClientAsync(sessionId);
            if (client == null) return NotFound();

            await _requestService.CreateRequestAsync(packageId, client.Id);

            TempData["Success"] = "Request created successfully!";
            return RedirectToAction("GetCoachingPackage", "CoachingPackages");
        }

      
        public IActionResult Payment()
        {
           return View();
        }
    }
}
