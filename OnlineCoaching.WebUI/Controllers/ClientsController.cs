using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.WebUI.Models.Clients;
using OnlineCoaching.WebUI.Models.RequestPackage;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize]
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

        #region Get all Clients in tables in dashboard of Admin
        public IActionResult Index()
        {
            var clients = _clientService.GetAllClients();
            return View(clients);
        }
        #endregion

        #region Profile of Client 

        public async Task<IActionResult> Profile(int id)
        {
            Client? client = null;

            if (id > 0) client = _clientService.GetClientById(id);

            else
            {
                var userId = User.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "Account");

                client = await _clientService.GetClientAsync(userId);
            }

            // Check if client has an active approved request
            var activeRequest = _requestService.GetActiveOrPendingRequest(client!.Id);
            bool showQuestions = activeRequest != null && activeRequest.Status == ClientStatus.Active;

            var questions = showQuestions ? _questionServices.GetQuestions() : new List<Question>();
            var answers = showQuestions ? _questionServices.GetClientAnswer(client.Id) : new List<ClientAnswer>();

            // Fetch assigned exercises and foods
            var assignedExercises = _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise).ThenInclude(m=>m!.Muscle)
                .Where(a => a.ClientId == client.Id)
                .ToList();

            var assignedFoods = _unitOfWork.AssignFoods
                .GetQueryable().Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == client.Id)
                .ToList();

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
                    VideoUrl = a.Exercise?.VideoUrl,
                    Sets = a.Sets,
                    Reps = a.Reps,
                    Notes = a.Notes,
                    DayOfWeek = a.DayOfWeek ,
                    MuscleName = a.Exercise?.Muscle?.Name ,
                    ImageUrl = a.Exercise?.ImageUrl 
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
        #endregion

        #region Profile of client for admin

        public async Task<IActionResult> ProfileDashboard(int id)
        {
            Client? client = null;

            if (id > 0) client = _clientService.GetClientById(id);

            else
            {
                var userId = User.GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "Account");

                client = await _clientService.GetClientAsync(userId);
            }

            // Check if client has an active approved request
            var activeRequest = _requestService.GetActiveOrPendingRequest(client!.Id);
            bool showQuestions = activeRequest != null && activeRequest.Status == ClientStatus.Active;

            var questions = showQuestions ? _questionServices.GetQuestions() : new List<Question>();
            var answers = showQuestions ? _questionServices.GetClientAnswer(client.Id) : new List<ClientAnswer>();

            // Fetch assigned exercises and foods
            var assignedExercises = _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise).ThenInclude(m => m!.Muscle)
                .Where(a => a.ClientId == client.Id)
                .ToList();

            var assignedFoods = _unitOfWork.AssignFoods
                .GetQueryable().Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == client.Id)
                .ToList();


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
                    DayOfWeek = a.DayOfWeek,
                    MuscleName = a.Exercise?.Muscle?.Name,
                    ImageUrl = a.Exercise?.ImageUrl
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

        #endregion


        #region CompleteProfileData
        [HttpPost]
        public async Task<IActionResult> CompleteProfile(Client client)
        {
            var userId = User.GetUserId();


            await _clientService.CompleteClientData(client, userId);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

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

    }
}
