using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.ExerciseSheet;

namespace OnlineCoaching.WebUI.Controllers
{
    public class ExerciseSheetLogsController : Controller
    {
        private readonly IExerciseServices _exerciseServices;

        public ExerciseSheetLogsController(IExerciseServices exerciseServices)
        {
            _exerciseServices = exerciseServices;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _exerciseServices.GetAllLogs();
            return View(logs);
        }

        public async Task<IActionResult> ClientExerciseLogs(int clientId, int exerciseId)
        {
            var logs = await _exerciseServices.GetLogsByClientAndExercise(clientId, exerciseId);

            ViewBag.ClientId = clientId;
            ViewBag.ExerciseId = exerciseId;

            // Always return a view, even if no logs
            return View("ClientExerciseLogs", logs);
        }


        public async Task<IActionResult> Details(int id)
        {
            var logs = await _exerciseServices.GetAllLogs();
            var log = logs.FirstOrDefault(l => l.Id == id);

            if (log == null) return NotFound();
            return View(log);
        }

        public IActionResult Create(int clientId, int exerciseId)
        {
            var dto = new CreateExerciseSheetDto
            {
                ClientId = clientId,
                ExerciseId = exerciseId
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExerciseSheetDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _exerciseServices.AddExerciseSheetLog(dto);
            return RedirectToAction(nameof(ClientExerciseLogs), new { clientId = dto.ClientId, exerciseId = dto.ExerciseId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var logs = await _exerciseServices.GetAllLogs();
            var log = logs.FirstOrDefault(l => l.Id == id);

            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExerciseSheetLog dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var updated = await _exerciseServices.UpdateExerciseSheetLog(dto);
            if (updated == null) return NotFound();

            return RedirectToAction(nameof(ClientExerciseLogs), new { clientId = dto.ClientId, exerciseId = dto.ExerciseId });
        }


        public async Task<IActionResult> Delete(int id)
        {
            var logs = await _exerciseServices.GetAllLogs();
            var log = logs.FirstOrDefault(l => l.Id == id);

            if (log == null) return NotFound();
            return View(log);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _exerciseServices.DeletedLogs(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
