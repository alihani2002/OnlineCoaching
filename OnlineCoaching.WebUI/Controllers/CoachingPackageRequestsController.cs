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

        // GET: Requests
        public IActionResult Index()
        {
            var requests = _requestService.GetRequests();
            return View(requests);
        }

        // GET: Requests/Details/5
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
                return RedirectToAction("Login", "Account"); // redirect guests to login
            }

            var client = await _clientService.GetClientAsync(sessionId);
            if (client == null) return NotFound("Client not found.");

            var requests = _requestService.GetUserRequests(client.Id);
            return View(requests); // will expect IEnumerable<CoachingPackageRequestDto>
        }


        // =============================
        // Approve Request (for Admin/Coach)
        // =============================
        //[HttpPost]
        //public async Task<IActionResult> ApproveRequest(int id)
        //{
        //    var success = await _requestService.ApproveRequestAsync(id);
        //    if (!success)
        //    {
        //        TempData["Error"] = "Failed to approve the request.";
        //        return RedirectToAction("PendingRequest", new { requestId = id });
        //    }

        //    TempData["Success"] = "Request approved successfully!";
        //    return RedirectToAction("Index", "CoachingPackage");
        //}

        // =============================
        // Reject Request (for Admin/Coach)
        // =============================
        //[HttpPost]
        //public async Task<IActionResult> RejectRequest(int id)
        //{
        //    var success = await _requestService.RejectRequestAsync(id);
        //    if (!success)
        //    {
        //        TempData["Error"] = "Failed to reject the request.";
        //        return RedirectToAction("PendingRequest", new { requestId = id });
        //    }

        //    TempData["Success"] = "Request rejected.";
        //    return RedirectToAction("Index", "CoachingPackage");
        //}

        //// GET: Requests/Create
        //public IActionResult Create(int packageId)
        //{
        //    ViewBag.PackageId = packageId;

        //    CreateCoachingPackageRequestDto request = new CreateCoachingPackageRequestDto()
        //    {
        //        PackageId = packageId
        //    };
        //    return View(request);
        //}

        //// POST: Requests/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(CreateCoachingPackageRequestDto dto)
        //{
        //    // get logged in user (clientId from claims)
        //    var clientId = User.GetUserId();


        //    if (!ModelState.IsValid) return View();

        //    await _requestService.AddRequestAsync(dto);
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Requests/EditStatus/5
        public async Task<IActionResult> EditStatus(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }

        // POST: Requests/EditStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, ClientStatus status)
        {
            var updated = await _requestService.UpdateStatusAsync(id, status);
            if (updated == null) return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);
            if (request == null) return NotFound();

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _requestService.DeleteRequestAsync(id);
            if (!deleted) return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // GET: Requests/ConfirmRequest
        public async Task<IActionResult> ConfirmRequest(int packageId)
        {
            // get logged in user id
            var sessionId = User.GetUserId();
            var client = await _clientService.GetClientAsync(sessionId); // ✅ await properly
            if (client == null) return NotFound();

            // get package info
            var package = await _coachingPackageServices.GetCoachingPackageByIdAsync(packageId);
            if (package == null) return NotFound();

            var model = new ConfirmRequestViewModel
            {
                PackageId = package.Id,
                PackageTitle = package.Title,
                PackagePrice = package.Price,
                DurationInMonths = package.DurationInMonths,
                ClientId = client.Id , // ✅ works now
                CreatedById = sessionId 
            };

            return View(model);
        }

        // POST: Requests/ConfirmRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRequest(ConfirmRequestViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _requestService.CreateRequestAsync(model.PackageId, model.ClientId);

            return RedirectToAction("Index", "Home");
        }
    }
}
