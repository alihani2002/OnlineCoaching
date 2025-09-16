using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionServices _questionServices;
            private readonly IClientService _clientService;
        private readonly ICoachingPackageRequestService _requestService;

        public QuestionController(IQuestionServices questionServices, IClientService clientService, ICoachingPackageRequestService requestService, ApplicationDbContext context)
            {
             _questionServices = questionServices;
            _clientService = clientService;
            _requestService = requestService;
            _context = context;
            }
        [Authorize(Roles = AppRoles.Admin)]

        public IActionResult Index()
            {
                var questions = _questionServices.GetQuestions();
                return View(questions);
            }
        [Authorize(Roles = AppRoles.Admin)]

        public async Task<IActionResult> Details(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }
        [Authorize(Roles = AppRoles.Admin)]

        public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]

        public IActionResult Create(Question question)
            {
                if (ModelState.IsValid)
                {
                    _questionServices.AddQuestionWithOption(question);
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
        [Authorize(Roles = AppRoles.Admin)]

        public async Task<IActionResult> Edit(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]

        public IActionResult Edit(Question question)
            {
                if (ModelState.IsValid)
                {
                    _questionServices.UpdateQuestion(question);
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
        [Authorize(Roles = AppRoles.Admin)]

        public async Task<IActionResult> Delete(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]

        public IActionResult DeleteConfirmed(int id)
            {
                _questionServices.DeleteQuestion(id);
                return RedirectToAction(nameof(Index));
            }

            #region Get action Questions of clients to answer it 
            // Clients/CompleteQuestion
            public IActionResult CompleteQuestion()
            {
                var questions = _questionServices.GetQuestions();
                return View(questions);
            }

            #endregion


            #region Post action Questions of clients to answer it 
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
                    // Skip empty answers completely
                    if (string.IsNullOrWhiteSpace(answer.AnswerText) &&
                        (answer.SelectedOptions == null || !answer.SelectedOptions.Any()))
                        continue;

                    answer.ClientId = client.Id;

                    if (!string.IsNullOrWhiteSpace(answer.AnswerText))
                    {
                        // text answer
                        answer.CreatedById = userId;
                        _context.ClientAnswers.Add(answer);
                    }
                    else if (answer.SelectedOptions != null && answer.SelectedOptions.Any())
                    {
                        // multiple-choice answers
                        var newAnswer = new ClientAnswer
                        {
                            CreatedById = userId,
                            ClientId = client.Id,
                            QuestionId = answer.QuestionId,
                            CreatedOn = DateTime.Now,
                        };

                        _context.ClientAnswers.Add(newAnswer);

                        foreach (var selectedOption in answer.SelectedOptions)
                        {
                            var option = new ClientAnswerOption
                            {
                                CreatedById = userId,
                                OptionId = selectedOption.OptionId,
                                ClientAnswer = newAnswer,
                                CreatedOn = DateTime.Now,
                            };
                            _context.ClientAnswerOptions.Add(option);
                        }
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

                return RedirectToAction("Profile" , "Clients");
            }

            #endregion

    }
}
