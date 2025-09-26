using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.WebUI.Models.RequestPackage;

namespace OnlineCoaching.WebUI.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class ExercisesController(IExerciseServices exerciseService,
        IMuscleServices muscleServices,
        IImageService imageHelper,
        IUnitOfWork unitOfWork,
        IAssignmentService assignmentService,
        ICoachingPackageRequestService requestService) : Controller
    {
        private readonly IExerciseServices _exerciseService = exerciseService;
        private readonly IMuscleServices _muscleServices = muscleServices;
        private readonly IImageService _imageHelper = imageHelper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAssignmentService _assignmentService = assignmentService;
        private readonly ICoachingPackageRequestService _requestService = requestService;


        #region  ------Exercises CRUD -------------------

        public IActionResult Index()
        {
            var exercises = _exerciseService.GetExercises();
            return View(exercises);
        }

        public async Task<IActionResult> Details(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            return exercise == null ? NotFound() : View(exercise);
        }

        public IActionResult Create()
        {
            PopulateMusclesDropdown();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExerciseDto dto, IFormFile? imageUrl)
        {
            if (!ModelState.IsValid)
            {
                PopulateMusclesDropdown();
                return View(dto);
            }

            dto.ImageUrl = imageUrl != null ?
                await _imageHelper.UploadImageAsync(imageUrl, "Exercises") : null;

            await _exerciseService.AddExerciseAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();

            PopulateMusclesDropdown(exercise.MuscleId);
            return View(exercise);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExerciseDto dto, IFormFile? imageUrl)
        {
            if (!ModelState.IsValid)
            {
                PopulateMusclesDropdown(dto.MuscleId);
                return View(dto);
            }

            dto.ImageUrl = imageUrl != null && imageUrl.Length > 0
                ? await _imageHelper.UploadImageAsync(imageUrl, "Exercises")
                : (await _exerciseService.GetExerciseByIdAsync(dto.Id))?.ImageUrl;

            dto.LastUpdatedOn = DateTime.Now;
            await _exerciseService.UpdateExerciseAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            return exercise == null ? NotFound() : View(exercise);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region -------------- Assign Exercises and Foods to Client ------------------
        public IActionResult Assign(int clientId)
        {
            var exercises = _unitOfWork.Exercises.GetQueryable();
            var foods = _unitOfWork.Foods.GetQueryable();
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
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> AssignedList(int clientId)
        {
            var model = new AssignedListViewModel
            {
                ClientId = clientId,
                AssignedExercises = await _assignmentService.GetAssignedExercisesAsync(clientId),
                AssignedFoods = await _assignmentService.GetAssignedFoodsAsync(clientId)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditAssigned(int clientId)
        {
            var request = await _requestService.GetRequestByIdAsyncNoTracking(clientId);

            if (request == null || request.Id == 0)
            {
                TempData["Error"] = "This client has no active coaching package request.";
                return RedirectToAction("Index");
            }

            var model = new AssignedListViewModel
            {
                ClientId = clientId,
                RequestId = request.Id,
                AssignedExercises = _assignmentService.GetAssignedExercise(clientId, request.Id),
                AssignedFoods = _assignmentService.GetAssignedFood(clientId, request.Id),
                AvailableExercises = _unitOfWork.Exercises.GetQueryable().ToList(),
                AvailableFoods = _unitOfWork.Foods.GetQueryable().ToList()
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
        #endregion

        private void PopulateMusclesDropdown(int? selectedMuscleId = null)
        {
            var muscles = _muscleServices.GetMuscles();
            ViewBag.Muscles = new SelectList(muscles, "Id", "Name", selectedMuscleId);
        }
    }
}
