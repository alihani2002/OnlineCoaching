using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Application.Services.AssignmentService;
using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.Domain.Entities;
using OnlineCoaching.Domain.Entities.Foods;
using OnlineCoaching.Domain.Enums;
using OnlineCoaching.WebUI.Models.Clients;
using OnlineCoaching.WebUI.Models.RequestPackage;

namespace OnlineCoaching.WebUI.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;
        private readonly IQuestionServices _questionServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICoachingPackageRequestService _requestService;
        public readonly IUnitOfWork _unitOfWork;
        private readonly IAssignmentService _assignmentService;

        public ClientsController(ApplicationDbContext context, IClientService clientService, UserManager<ApplicationUser> userManager, IQuestionServices questionServices, ICoachingPackageRequestService requestService, IUnitOfWork unitOfWork, IAssignmentService assignmentService)
        {
            _context = context;
            _clientService = clientService;
            _userManager = userManager;
            _questionServices = questionServices;
            _requestService = requestService;
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
        }

        //Get all Clients in tables
        public IActionResult Index()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }


        //Profile of Client
        public async Task<IActionResult> Profile(int id)
        {

            Client? client = null;

            if (id > 0)
                client = _clientService.GetClientById(id);

            else
            {
                // Normal user views their own profile
                var userId = User.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account"); // not logged in
                }

                client = await _clientService.GetClientAsync(userId);
            }



            if (client == null)
                return NotFound("Client not found.");

            // Check if client has an active approved request
            var activeRequest = _requestService.GetActiveOrPendingRequest(client.Id);
            bool showQuestions = activeRequest != null && activeRequest.Status == ClientStatus.Active;

            var questions = showQuestions ? _questionServices.GetQuestions() : new List<Question>();
            var answers = showQuestions ? _questionServices.GetClientAnswer(client.Id) : new List<ClientAnswer>();



            // Fetch assigned exercises and foods
            var assignedExercises = _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise)
                .Where(a => a.ClientId == client.Id)
                .ToList();

            var assignedFoods = _unitOfWork.AssignFoods
                .GetQueryable().Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == client.Id)
                .ToList();

                 var exercisesByDay = assignedExercises
                .GroupBy(e => e.DayOfWeek)
                .ToDictionary(g => g.Key, g => g.ToList());

            var foodsByMeal = assignedFoods
                .GroupBy(f => f.MealNumber)
                .ToDictionary(g => g.Key, g => g.ToList());


            var vm = new ClientDashboardViewModel
            {
                Client = client,
                Questions = questions,
                Answers = answers,
                AssignedExercises = assignedExercises.Select(a => new AssignExerciseDto
                {
                    Id = a.Id,
                    ExerciseId = a.ExerciseId,
                    NameOfExercise = a.Exercise?.Name,
                    Sets = a.Sets,
                    Reps = a.Reps,
                    Notes = a.Notes,
                    DayOfWeek = a.DayOfWeek
                }).ToList(),

                AssignedFoods = assignedFoods.Select(f => new AssignFoodDto
                {
                    Id = f.Id,
                    FoodId = f.FoodId,
                    FoodName = f.Food?.Name,
                    Quantity = f.Quantity,
                    Notes = f.Notes,
                    NumberOfServings = f.NumberOfServings,
                    DayOfWeek = f.Meal?.DayOfWeek ?? f.DayOfWeek,
                    MealNumber = f.Meal?.MealNumber ?? f.MealNumber,

                    DayOfWeekName = (f.Meal?.DayOfWeek ?? f.DayOfWeek).ToString(),
                    MealNumberName = (f.Meal?.MealNumber ?? f.MealNumber).ToString(),
                }).ToList(),
              

            };

            return View(vm);
        }

        // Clients/CompleteQuestion
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
                        QuestionId = answer.QuestionId ,
                        CreatedOn = DateTime.Now,
                    };

                    _context.ClientAnswers.Add(newAnswer);

                    foreach (var selectedOption in answer.SelectedOptions)
                    {
                        var option = new ClientAnswerOption
                        {
                            CreatedById = userId,
                            OptionId = selectedOption.OptionId,
                            ClientAnswer = newAnswer , 
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

            return RedirectToAction("Profile");
        }


        // Clients/CompleteProfileData

        [HttpPost]
        public async Task<IActionResult> CompleteProfile(Client client)
        {
            var userId = User.GetUserId();

            await _clientService.CompleteClientData(client, userId);
            return RedirectToAction("Index", "Home");
        }



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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid client id.");

            var result = await _clientService.ToggleDeleteAsync(id);

            if (!result)
                return NotFound();

            TempData["Success"] = "Client status updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }


        public IActionResult Assign(int clientId)
        {
            var exercises = _unitOfWork.Exercises.GetAll();
            var foods = _unitOfWork.Foods.GetAll();
            var request = _requestService.GetUserRequest(clientId).Result;

            var model = new AssignViewModel
            {
                ClientId = clientId,
                RequestId = request?.Id ?? 0,
                AvailableExercises = exercises.ToList(),
                AvailableFoods = foods.ToList(),
                Exercises = new List<AssignExerciseDto> { new AssignExerciseDto() },
                Foods = new List<AssignFoodDto> { new AssignFoodDto() }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Exercises != null && model.Exercises.Any())
                await _assignmentService.AssignExercisesAsync(model.ClientId, model.Exercises);

            if (model.Foods != null && model.Foods.Any())
                await _assignmentService.AssignFoodsAsync(model.ClientId, model.Foods);

            return RedirectToAction("AssignedList", new { clientId = model.ClientId, requestId = model.RequestId });
        }


        [HttpGet]
        public IActionResult AssignedList(int clientId)
        {
            var assignedExercises = _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise)
                .Where(a => a.ClientId == clientId)
                .ToList();

            var assignedFoods = _unitOfWork.AssignFoods
                .GetQueryable()
                .Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == clientId)
                .ToList();

            var model = new AssignedListViewModel
            {
                ClientId = clientId,
                AssignedExercises = assignedExercises,
                AssignedFoods = assignedFoods
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult EditAssigned(int clientId)
        {
            var requestId = _unitOfWork.CoachingPackageRequests
                .GetQueryable()
                .Where(r => r.ClientId == clientId && r.Status == ClientStatus.Active) // or Pending
                .Select(r => r.Id)
                .FirstOrDefault();

            if (requestId == 0)
            {
                TempData["Error"] = "This client has no active coaching package request.";
                return RedirectToAction("Index");
            }

            var assignedExercises = _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise)
                .Where(a => a.ClientId == clientId && a.CoachingPackageRequestId == requestId)
                .ToList();

            var assignedFoods = _unitOfWork.AssignFoods
                .GetQueryable()
                .Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == clientId && f.CoachingPackageRequestId == requestId)
                .ToList();

            var model = new AssignedListViewModel
            {
                ClientId = clientId,
                RequestId = requestId,   // ✅ ensure it’s set
                AssignedExercises = assignedExercises,
                AssignedFoods = assignedFoods,
                AvailableExercises = _unitOfWork.Exercises.GetAll().ToList(),
                AvailableFoods = _unitOfWork.Foods.GetAll().ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssigned(AssignedListViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // --- Exercises ---
            var dbExercises = _unitOfWork.AssignExercises.GetQueryable().Where(a => a.ClientId == model.ClientId).ToList();
            if (model.AssignedExercises == null || !model.AssignedExercises.Any())
            {
                foreach (var dbEx in dbExercises) _unitOfWork.AssignExercises.Remove(dbEx);
            }
            else
            {
                foreach (var dbEx in dbExercises)
                {
                    var updatedEx = model.AssignedExercises.FirstOrDefault(x => x.Id == dbEx.Id);
                    if (updatedEx != null)
                    {
                        dbEx.ExerciseId = updatedEx.ExerciseId;
                        dbEx.Sets = updatedEx.Sets;
                        dbEx.Reps = updatedEx.Reps;
                        dbEx.Notes = updatedEx.Notes;
                        dbEx.DayOfWeek = updatedEx.DayOfWeek;
                        _unitOfWork.AssignExercises.Update(dbEx);
                    }
                    else _unitOfWork.AssignExercises.Remove(dbEx);
                }

                var newExercises = model.AssignedExercises.Where(x => x.Id == 0).ToList();
                foreach (var ex in newExercises)
                {
                    ex.ClientId = model.ClientId;
                    ex.CoachingPackageRequestId = model.RequestId;
                    ex.AssignedOn = DateTime.UtcNow;
                    _unitOfWork.AssignExercises.Add(ex);
                }
            }

            // --- Foods ---
            var dbFoods = _unitOfWork.AssignFoods
                .GetQueryable()
                .Include(f => f.Meal)
                .Where(f => f.ClientId == model.ClientId)
                .ToList();

            if (model.AssignedFoods == null || !model.AssignedFoods.Any())
            {
                foreach (var dbFood in dbFoods)
                    _unitOfWork.AssignFoods.Remove(dbFood);
            }
            else
            {
                foreach (var dbFood in dbFoods)
                {
                    var updatedFood = model.AssignedFoods.FirstOrDefault(x => x.Id == dbFood.Id);
                    if (updatedFood != null)
                    {
                        dbFood.FoodId = updatedFood.FoodId;
                        dbFood.Quantity = updatedFood.Quantity;
                        dbFood.Notes = updatedFood.Notes;
                        dbFood.DayOfWeek = updatedFood.DayOfWeek;
                        dbFood.MealNumber = updatedFood.MealNumber;
                        dbFood.NumberOfServings = updatedFood.NumberOfServings;

                        // ensure correct Meal is linked
                        var meal = _unitOfWork.Meals.GetQueryable()
                            .FirstOrDefault(m =>
                                m.ClientId == model.ClientId &&
                                m.CoachingPackageRequestId == model.RequestId &&
                                m.DayOfWeek == updatedFood.DayOfWeek &&
                                m.MealNumber == updatedFood.MealNumber);

                        if (meal == null)
                        {
                            meal = new Meal
                            {
                                ClientId = model.ClientId,
                                CoachingPackageRequestId = model.RequestId,
                                DayOfWeek = updatedFood.DayOfWeek,
                                MealNumber = updatedFood.MealNumber,
                                CreatedOn = DateTime.UtcNow
                            };
                            _unitOfWork.Meals.Add(meal);
                            _unitOfWork.Complete();
                        }

                        dbFood.MealId = meal.Id;
                        _unitOfWork.AssignFoods.Update(dbFood);
                    }
                    else _unitOfWork.AssignFoods.Remove(dbFood);
                }

                var newFoods = model.AssignedFoods.Where(x => x.Id == 0).ToList();
                foreach (var f in newFoods)
                {
                    var meal = _unitOfWork.Meals.GetQueryable()
                        .FirstOrDefault(m =>
                            m.ClientId == model.ClientId &&
                            m.CoachingPackageRequestId == model.RequestId &&
                            m.DayOfWeek == f.DayOfWeek &&
                            m.MealNumber == f.MealNumber);

                    if (meal == null)
                    {
                        meal = new Meal
                        {
                            ClientId = model.ClientId,
                            CoachingPackageRequestId = model.RequestId,
                            DayOfWeek = f.DayOfWeek,
                            MealNumber = f.MealNumber,
                            CreatedOn = DateTime.UtcNow
                        };
                        _unitOfWork.Meals.Add(meal);
                        _unitOfWork.Complete();
                    }

                    var newFood = new AssignFood
                    {
                        ClientId = model.ClientId,
                        CoachingPackageRequestId = model.RequestId,
                        FoodId = f.FoodId,
                        Quantity = f.Quantity,
                        Notes = f.Notes,
                        DayOfWeek = f.DayOfWeek,
                        MealNumber = f.MealNumber,
                        MealId = meal.Id,
                        AssignedOn = DateTime.UtcNow,
                        CreatedOn = DateTime.UtcNow
                    };

                    _unitOfWork.AssignFoods.Add(newFood);
                }
            }

            _unitOfWork.Complete();

            TempData["Success"] = "Assignments updated successfully.";
            return RedirectToAction("AssignedList", new { clientId = model.ClientId, requestId = model.RequestId });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAssignedExercise(int id, int clientId)
        {
            await _assignmentService.DeleteAssignedExerciseAsync(id);
            TempData["Success"] = "Exercise removed successfully.";
            return RedirectToAction("EditAssigned", new { clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignedFood(int id, int clientId)
        {
            await _assignmentService.DeleteAssignedFoodAsync(id);
            TempData["Success"] = "Food removed successfully.";
            return RedirectToAction("EditAssigned", new { clientId });
        }
    }
}
