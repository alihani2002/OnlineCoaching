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
    public HomeController(ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        ICoachingPackageRequestService coachingPackageRequestService 
        , IQuestionServices questionServices , IClientService clientService)
    {
        _logger = logger;
        _userManager = userManager;
        _coachingPackageRequestService = coachingPackageRequestService;
        _questionServices = questionServices;
        _clientService = clientService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null && !user.IsCompelteProfile)
        {
            ViewBag.ShowProfilePopup = true;
        }

        if (user != null)
        {
            var client = await _clientService.GetClientAsync(user.Id);

            if (client != null)
            {
                var existingRequest = _coachingPackageRequestService.GetActiveOrPendingRequest(client!.Id);
                if (existingRequest != null)
                    if (existingRequest.IsAnswerQuestion == false && existingRequest.Status == ClientStatus.Active)
                        return RedirectToAction("CompleteQuestion", "Question", new { id = existingRequest.Id });
                    else
                        return View();
            }
        }
        return View();
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
