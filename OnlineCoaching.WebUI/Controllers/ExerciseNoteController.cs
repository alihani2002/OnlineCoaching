using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.Notes;
using System.Security.Claims;

namespace OnlineCoaching.WebUI.Controllers
    {
        [Authorize]
        public class ExerciseNoteController : Controller
        {
            private readonly IExerciseNoteServices _noteService;
        private readonly IClientService _clientService;
        

            public ExerciseNoteController(IExerciseNoteServices noteService , IClientService clientService)
            {
                _noteService = noteService;
                _clientService = clientService;
        }

            // Show all notes for a given exercise
            [HttpGet]
            public async Task<IActionResult> Index(int exerciseId)
            {
                var notes = await _noteService.GetNotesByExerciseAsync(exerciseId);
                ViewBag.ExerciseId = exerciseId;
                return View(notes);
            }
        [HttpGet]
        public async Task<IActionResult> AllNotes()
        {
            var notes = await _noteService.GetAllWithDetailsAsync();
            return View(notes);
        }

        // GET: Add Note
        [HttpGet]
            public IActionResult Create(int exerciseId)
            {
                var model = new CreateExerciseNoteDto { ExerciseId = exerciseId };
                return View(model);
            }

            // POST: Add Note
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateExerciseNoteDto dto)
            {
                if (!ModelState.IsValid)
                    return View(dto);


            var userId = User.GetUserId();
            var loggedInClient = await _clientService.GetClientAsync(userId);

                if (string.IsNullOrEmpty(userId))
                        return Unauthorized();
                    dto.ClientId = loggedInClient!.Id;

                await _noteService.AddAsync(dto);
                return RedirectToAction(nameof(Index), new { exerciseId = dto.ExerciseId });
            }

            // GET: Delete
            [HttpGet]
            public async Task<IActionResult> Delete(int id)
            {
                var note = await _noteService.GetByIdAsync(id);
                if (note == null) return NotFound();

                // ✅ ensure only owner or admin can delete
                var clientIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (note.ClientId.ToString() != clientIdClaim && !User.IsInRole("Admin"))
                    return Forbid();

                return View(note);
            }

            // POST: Delete
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id, int exerciseId)
            {
                await _noteService.DeleteAsync(id);
                return RedirectToAction(nameof(Index), new { exerciseId });
            }
        }
    }

