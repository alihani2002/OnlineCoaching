using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCoaching.Application.Services;
using OnlineCoaching.WebUI.Models.Clients;

namespace OnlineCoaching.WebUI.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;
        private readonly IQuestionServices _questionServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICoachingPackageRequestService _requestService;

        public ClientsController(ApplicationDbContext context , IClientService clientService, UserManager<ApplicationUser> userManager, IQuestionServices questionServices, ICoachingPackageRequestService requestService)
        {
            _context = context;
            _clientService = clientService;
            _userManager = userManager;
            _questionServices = questionServices;
            _requestService = requestService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            var client = await _clientService.GetClientAsync(userId);

            if (client == null)
                return NotFound("Client not found.");

            var user = await _userManager.GetUserAsync(User);
            if (user != null && !user.IsCompelteProfile)
            {
                ViewBag.ShowProfilePopup = true;
            }

            var questions = _questionServices.GetQuestions();
            var answers = _questionServices.GetClientAnswer(client.Id);
                //await _context.ClientAnswers
                //.Include(a => a.SelectedOptions)
                //.ThenInclude(o => o.Option)
                //.Where(a => a.ClientId == client.Id)
                //.ToListAsync();

            var vm = new ClientDashboardViewModel
            {
                Client = client,
                Questions = questions,
                Answers = answers
            };

            return View(vm);
        }


        // GET: Clients/CompleteQuestion
        public IActionResult CompleteQuestion()
        {
            var questions = _questionServices.GetQuestions();
            return View(questions);
        }

        // POST: Clients/CompleteQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteQuestion(List<ClientAnswer> answers)
        {
            var userId = User.GetUserId();
            var client = await _clientService.GetClientAsync(userId);
            var existingRequest = _requestService.GetActiveOrPendingRequest(client!.Id);
           

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            foreach (var answer in answers)
            {
                answer.ClientId = client.Id;

                // If it's text-based answer
                if (!string.IsNullOrWhiteSpace(answer.AnswerText))
                {
                    _context.ClientAnswers.Add(answer);
                }
                else if (answer.SelectedOptions != null && answer.SelectedOptions.Any())
                {
                    // If multiple-choice or single-choice
                    foreach (var selectedOption in answer.SelectedOptions)
                    {
                        selectedOption.ClientAnswer = answer;
                    }
                    _context.ClientAnswers.Add(answer);
                }
            }
            if (existingRequest != null)
            {               
                var coachingPackageRequestEntity = _context.CoachingPackageRequests
                    .FirstOrDefault(cpr => cpr.Id == existingRequest.Id);

                if (coachingPackageRequestEntity != null)
                {
                    coachingPackageRequestEntity.IsAnswerQuestion = true;
                    _context.CoachingPackageRequests.Update(coachingPackageRequestEntity);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CompleteProfile(Client client)
        {
            var userId = User.GetUserId();

            await _clientService.CompleteClientData(client , userId);
            return RedirectToAction("Index");
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,BirthDate,PhoneNumber,Address,UserId,Id,IsDeleted,CreatedById,CreatedOn,LastUpdatedById,LastUpdatedOn")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,BirthDate,PhoneNumber,Address,UserId,Id,IsDeleted,CreatedById,CreatedOn,LastUpdatedById,LastUpdatedOn")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", client.UserId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
