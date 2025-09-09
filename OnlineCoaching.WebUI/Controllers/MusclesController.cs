using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.WebUI.Controllers
{

    [Authorize(Roles = AppRoles.Admin)]
    public class MusclesController : Controller
    {
        private readonly IMuscleServices _muscleService;

        public MusclesController(IMuscleServices muscleService)
        {
            _muscleService = muscleService;
        }

        public IActionResult Index()
        {
            var muscles = _muscleService.GetMuscles();
            return View(muscles);
        }

        public async Task<IActionResult> Details(int id)
        {
            var muscle = await _muscleService.GetMuscleByIdAsync(id);
            if (muscle == null) return NotFound();
            return View(muscle);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMuscleDto dto)
        {
            if (ModelState.IsValid)
            {
                await _muscleService.AddMuscleAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var muscle = await _muscleService.GetMuscleByIdAsync(id);
            if (muscle == null) return NotFound();
            return View(muscle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MuscleDto dto)
        {
            if (ModelState.IsValid)
            {
                await _muscleService.UpdateMuscleAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var muscle = await _muscleService.GetMuscleByIdAsync(id);
            if (muscle == null) return NotFound();
            return View(muscle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _muscleService.DeleteMuscleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
