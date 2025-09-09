using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class QuestionController : Controller
    {
            private readonly IQuestionServices _questionServices;

            public QuestionController(IQuestionServices questionServices)
            {
                _questionServices = questionServices;
            }

            public IActionResult Index()
            {
                var questions = _questionServices.GetQuestions();
                return View(questions);
            }

            public async Task<IActionResult> Details(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Question question)
            {
                if (ModelState.IsValid)
                {
                    _questionServices.AddQuestionWithOption(question);
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Question question)
            {
                if (ModelState.IsValid)
                {
                    _questionServices.UpdateQuestion(question);
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                _questionServices.DeleteQuestion(id);
                return RedirectToAction(nameof(Index));
            }
        }
}
