using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCoaching.Application;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.WebUI.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly IExerciseServices _exerciseService;
        private readonly IMuscleServices _muscleServices;

        public ExercisesController(IExerciseServices exerciseService , IMuscleServices muscleServices)
        {   
            _exerciseService = exerciseService;
            _muscleServices = muscleServices;
        }

        public IActionResult Index()
        {
            var exercises = _exerciseService.GetExercises();
            return View(exercises);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();

            return View(exercise);
        }

        public IActionResult Create()
        {
            var muscles = _muscleServices.GetMuscles();

            ViewBag.Muscles = new SelectList(muscles, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExerciseDto dto)
        {
            if (ModelState.IsValid)
            {
                await _exerciseService.AddExerciseAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            var muscles = _muscleServices.GetMuscles();
            ViewBag.Muscles = new SelectList(muscles, "Id", "Name");

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();

            ViewBag.Muscles = new SelectList(_muscleServices.GetMuscles(), "Id", "Name", exercise.MuscleId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExerciseDto dto)
        {
            if (ModelState.IsValid)
            {
                await _exerciseService.UpdateExerciseAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Muscles = new SelectList(_muscleServices.GetMuscles(), "Id", "Name", dto.MuscleId);
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();

            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
