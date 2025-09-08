using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize(AppRoles.Admin)]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminRegister()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminRegister(ApplicationUser dto)
        {
            var userId = User.GetUserId();
            await _authService.RegisterAdminUser(dto , userId);
            return RedirectToAction("Index");
        }
    }
}
