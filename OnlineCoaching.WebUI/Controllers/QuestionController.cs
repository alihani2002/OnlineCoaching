using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
            private readonly IQuestionServices _questionServices;

            public QuestionController(IQuestionServices questionServices)
            {
                _questionServices = questionServices;
            }

            // GET: /Question
            public IActionResult Index()
            {
                var questions = _questionServices.GetQuestions();
                return View(questions);
            }

            // GET: /Question/Details/5
            public async Task<IActionResult> Details(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            // GET: /Question/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: /Question/Create
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

            // GET: /Question/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            // POST: /Question/Edit/5
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

            // GET: /Question/Delete/5
            public async Task<IActionResult> Delete(int id)
            {
                var question = await _questionServices.GetQuestionById(id);
                if (question == null)
                    return NotFound();

                return View(question);
            }

            // POST: /Question/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                _questionServices.DeleteQuestion(id);
                return RedirectToAction(nameof(Index));
            }
        }
}
