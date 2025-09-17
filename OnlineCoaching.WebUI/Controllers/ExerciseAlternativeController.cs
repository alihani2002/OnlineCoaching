using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.WebUI.Controllers
{
    namespace OnlineCoaching.Web.Controllers
    {
        public class ExerciseAlternativeController : Controller
        {
            private readonly IExerciseAlternativeService _exerciseAlternativeService;
            private readonly IExerciseServices _exerciseService;

            public ExerciseAlternativeController(
                IExerciseAlternativeService exerciseAlternativeService,
                IExerciseServices exerciseService)
            {
                _exerciseAlternativeService = exerciseAlternativeService;
                _exerciseService = exerciseService;
            }

            // ✅ عرض البدائل لتمرين معين
            public async Task<IActionResult> Index()
            {
                var exercises = await _exerciseAlternativeService.GetAllExercisesWithAlternativesAsync();

                return View(exercises);
            }

            public async Task<IActionResult> ExerciseAlternativeProfile()
            {
                var exercises = await _exerciseAlternativeService.GetAllExercisesWithAlternativesAsync();

                return View(exercises);
            }

            // ✅ عرض فورم إضافة بديل
            [HttpGet]
            public async Task<IActionResult> Create(int exerciseId)
            {
                var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
                if (exercise == null) return NotFound();

                ViewBag.ExerciseName = exercise.Name;
                ViewBag.ExerciseId = exercise.Id;

                // ✅ Get existing alternatives for this exercise
                var existingAlternatives = await _exerciseAlternativeService.GetAlternativesByExerciseIdAsync(exerciseId);
                var existingAlternativeIds = existingAlternatives.Select(a => a.AlternativeExerciseId).ToList();

                // ✅ Get all exercises but exclude:
                // - the same exercise
                // - already added alternatives
                var allExercises = _exerciseService.GetExercises()
                                    .Where(e => e.Id != exerciseId && !existingAlternativeIds.Contains(e.Id))
                                    .ToList();

                ViewBag.AllExercises = allExercises;

                return View(new CreateExerciseAlternativeDto { ExerciseId = exerciseId });
            }


            // ✅ إضافة بديل
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateExerciseAlternativeDto dto)
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }

                try
                {
                    await _exerciseAlternativeService.AddAlternativeAsync(dto);
                    return RedirectToAction(nameof(Index), new { exerciseId = dto.ExerciseId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(dto);
                }
            }

            // ✅ حذف بديل
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Delete(int exerciseId, int alternativeExerciseId)
            {
                var result = await _exerciseAlternativeService.DeleteAlternativeAsync(exerciseId, alternativeExerciseId);
                if (!result) return NotFound();

                return RedirectToAction(nameof(Index), new { exerciseId });
            }
        }
    }
}
