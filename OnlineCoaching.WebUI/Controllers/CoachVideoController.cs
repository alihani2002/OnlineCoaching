using Microsoft.AspNetCore.Mvc;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachVideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
