using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.WebUI.Models.RequestPackage;

namespace OnlineCoaching.WebUI.Controllers
{
    public class CoachingAssignmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssignmentService _assignmentService;
        private readonly IMapper _mapper;

        public CoachingAssignmentController(IUnitOfWork unitOfWork, IAssignmentService assignmentService , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int packageId)
        {
            var request = _assignmentService.GetAssignedPackageFree(packageId);

            if (request == null)
            {
                // Handle the case where a free package has no existing assignments.
                // We fetch the package title and create a new, empty view model.
                var packageEntity = await _unitOfWork.CoachingPackages.GetByIdAsync(packageId);
                ViewBag.PackageTitle = packageEntity?.Title;

                var emptyModel = new AssignedFreePackVM
                {
                    PackageId = packageId,
                    AssignedExercises = new List<AssignExercise>(),
                    AssignedFoods = new List<AssignFood>(),
                    AvailableExercises = _unitOfWork.Exercises.GetQueryable().ToList(),
                    AvailableFoods = _unitOfWork.Foods.GetQueryable().ToList()
                };
                return View(emptyModel);
            }

            var model = _mapper.Map<AssignedFreePackVM>(request);
            model.AvailableExercises = _unitOfWork.Exercises.GetQueryable().ToList();
            model.AvailableFoods = _unitOfWork.Foods.GetQueryable().ToList();
            ViewBag.PackageTitle = request.Package?.Title;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FreePackDetails(int packageId)
        {
            var request = _assignmentService.GetAssignedPackageFree(packageId);
            var packageTitle = (await _unitOfWork.CoachingPackages.GetByIdAsync(packageId))?.Title;

            if (request == null)
            {
                var packageEntity = await _unitOfWork.CoachingPackages.GetByIdAsync(packageId);
                var entity = new AssignedFreePackVM() 
                {
                PackageId= packageId,
                AssignedExercises = new List<AssignExercise>(),
                AssignedFoods= new List<AssignFood>(),
                };

                ViewBag.PackageTitle = packageEntity?.Title;
                return View(entity);

            }
            var model = _mapper.Map<AssignedFreePackVM>(request);
            ViewBag.PackageTitle = packageTitle;
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Assign(int packageId)
        {
            var package = _assignmentService.GetAssignedPackageFree(packageId);
            var packageTitle = (await _unitOfWork.CoachingPackages.GetByIdAsync(packageId))?.Title;

            var model = new AssignFreePackageViewModel
            {
                PackageId = packageId,
                Exercises = _mapper.Map<List<AssignExerciseDto>>(package?.AssignExercises ?? new List<AssignExercise>()),
                Foods = _mapper.Map<List<AssignFoodDto>>(package?.AssignFoods ?? new List<AssignFood>()),
                AvailableExercises = _unitOfWork.Exercises.GetQueryable().ToList(),
                AvailableFoods = _unitOfWork.Foods.GetQueryable().ToList()
            };
            ViewBag.PackageTitle = packageTitle;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignFreePackageViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Exercises.Any())
            {
                await _assignmentService.AssignExercisesToFreePackageAsync(model.PackageId, model.Exercises);
            }

            if (model.Foods.Any())
            {
                await _assignmentService.AssignFoodsToFreePackageAsync(model.PackageId, model.Foods);
            }

            TempData["Success"] = "Free package updated successfully.";
            return RedirectToAction("Manage", new { packageId = model.PackageId });
        }




        [HttpPost]
        public async Task<IActionResult> DeleteExerciseFromPackage(int id, int packageId)
        {
            await _assignmentService.DeleteAssignedExerciseAsync(id);
            TempData["Success"] = "Exercise deleted from package.";
            return RedirectToAction("Manage", new { packageId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFoodFromPackage(int id, int packageId)
        {
            await _assignmentService.DeleteAssignedFoodAsync(id);
            TempData["Success"] = "Food deleted from package.";
            return RedirectToAction("Manage", new { packageId });
        }
    }
}
