using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.WebUI.Models;
using System.Diagnostics;

namespace OnlineCoaching.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IQuestionServices _questionServices;
    private readonly ICoachingPackageRequestService _coachingPackageRequestService;
    private readonly IClientService _clientService;
    private readonly ICoachingPackageServices _coachingPackage;
    public HomeController(ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        ICoachingPackageRequestService coachingPackageRequestService 
        , IQuestionServices questionServices, IClientService clientService, ICoachingPackageServices coachingPackage)
    {
        _logger = logger;
        _userManager = userManager;
        _coachingPackageRequestService = coachingPackageRequestService;
        _questionServices = questionServices;
        _clientService = clientService;
        _coachingPackage = coachingPackage;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var freePack = _coachingPackage.GetPackageFree();
        Client client = null;

        if (user != null)
        {
            // 1. Try to retrieve existing client data
            client = await _clientService.GetClientAsync(user!.Id);

            if (client != null)
            {
                // 2. Check for active/pending request and redirect if question needs completion
                var existingRequest = _coachingPackageRequestService.GetActiveOrPendingRequest(client.Id);

                if (existingRequest != null && existingRequest.IsAnswerQuestion == false && existingRequest.Status == ClientStatus.Active)
                {
                    return RedirectToAction("CompleteQuestion", "Question", new { id = existingRequest.Id });
                }
            }

            // 3. If the user is logged in and hasn't completed their profile, show the popup.
            //    We check this after the potential redirection to ensure we don't show the popup needlessly.
            if (!user.IsCompelteProfile)
            {
                ViewBag.ShowProfilePopup = true;
            }
        }

        // 4. Construct the ViewModel, using the retrieved client data (for prepopulating the form) 
        //    or a new Client instance if no client was found or the user isn't logged in.
        var vm = new HomeIndexViewModel
        {
            CoachingPackages = freePack,
            Client = client ?? new Client()
        };

        // 5. Return the view with the fully initialized ViewModel, 
        //    ensuring the partial view call in Index.cshtml has the correct model to reference.
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }



    [Route("Home/StatusCode")]
    public IActionResult StatusCodeHandler(int code)
    {
        switch (code)
        {
            case 404:
                return View("FourHunderedFour"); // Views/Shared/FourHunderedFour.cshtml
            case 403:
                return View("Forbidden"); // optional
            case 500:
                return View("ServerError"); // optional
            default:
                return View("Error");
        }
    }


    public IActionResult FourHunderedFour()
    {
        return View();
    }
}
