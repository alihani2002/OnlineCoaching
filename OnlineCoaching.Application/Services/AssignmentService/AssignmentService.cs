using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.Domain.Entities;
using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICoachingPackageRequestService _coachingPackageRequestService;

        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper, ICoachingPackageRequestService coachingPackageRequestService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _coachingPackageRequestService = coachingPackageRequestService;
        }

        // ------------------ Get Assigned Exercise ------------------
        public List<AssignExercise> GetAssignedExercise(int clientId , int requestId)
        {
            var entity =  _unitOfWork.AssignExercises
                .GetQueryable().Include(a => a.Exercise)
                .Where(a => a.ClientId == clientId && a.CoachingPackageRequestId == requestId)
                .ToList();
            return entity!;
        }

        // ------------------ Get Assigned Food ------------------
        public List<AssignFood> GetAssignedFood(int clientId, int requestId)
        {
            var entity = _unitOfWork.AssignFoods
                .GetQueryable().Include(a => a.Food).Include(a => a.Meal)
                .Where(a => a.ClientId == clientId && a.CoachingPackageRequestId == requestId)
                .ToList();
            return entity!;
        }
  

        // ------------------ Get Assigned Package Free ------------------
        public CoachingPackageRequest GetAssignedPackageFree(int packageId)
        {
            var entity = _unitOfWork.CoachingPackageRequests
               .GetQueryable()
               .Include(r => r.AssignExercises!).ThenInclude(a => a.Exercise).ThenInclude(e => e!.Muscle)
               .Include(r => r.AssignFoods!).ThenInclude(a => a.Food)
               .Include(e => e.Package)
               .FirstOrDefault(r => r.PackageId == packageId && r.ClientId == null); // <--- Key filter
            return entity!;
        }

        // ------------------ Exercises ------------------
        public async Task AssignExercisesAsync(int clientId, List<AssignExerciseDto> exercises)
        {

            var request = await _coachingPackageRequestService.GetUserRequest(clientId);

            foreach (var ex in exercises)
            {
                var assignment = new AssignExercise
                {
                    ClientId = clientId,
                    CoachingPackageRequestId = request!.Id,
                    ExerciseId = ex.ExerciseId,
                    Sets = ex.Sets,
                    Reps = ex.Reps,
                    Notes = ex.Notes,
                    AssignedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    DayOfWeek = ex.DayOfWeek ,
                    SelectedDays = ex.SelectedDays ?? new List<int>(),
                };

                await _unitOfWork.AssignExercises.AddAsync(assignment);
            }

            _unitOfWork.Complete();
        }

        public async Task AssignExercisesToFreePackageAsync(int packageId, List<AssignExerciseDto> exercises)
        {
            var request = _unitOfWork.CoachingPackageRequests
                .GetQueryable()
                .FirstOrDefault(r => r.PackageId == packageId && r.ClientId == null);

            if (request == null)
            {
                request = new CoachingPackageRequest
                {
                    PackageId = packageId,
                    Titles = (await _unitOfWork.CoachingPackages.GetByIdAsync(packageId))?.Title,
                    Status = ClientStatus.Active,
                    CreatedOn = DateTime.UtcNow
                };

                await _unitOfWork.CoachingPackageRequests.AddAsync(request);
                _unitOfWork.Complete();
            }

            foreach (var dto in exercises)
            {
                var assignment = _mapper.Map<AssignExercise>(dto);
                assignment.CoachingPackageRequestId = request.Id;

                await _unitOfWork.AssignExercises.AddAsync(assignment);
            }

            _unitOfWork.Complete();
        }

        // ------------------ Foods ------------------
        public async Task AssignFoodsAsync(int clientId, List<AssignFoodDto> foods)
        {
            var request = await _coachingPackageRequestService.GetUserRequest(clientId);

            foreach (var food in foods)
            {
                // 🔹 Find or create the correct meal for this day + meal number
                var meal = _unitOfWork.Meals
                    .GetQueryable()
                    .FirstOrDefault(m =>
                        m.ClientId == clientId &&
                        m.CoachingPackageRequestId == request!.Id &&
                        m.DayOfWeek == food.DayOfWeek &&
                        m.MealNumber == food.MealNumber);

                if (meal == null)
                {
                    meal = new Meal
                    {
                        ClientId = clientId,
                        CoachingPackageRequestId = request!.Id,
                        DayOfWeek = food.DayOfWeek,
                        MealNumber = food.MealNumber,
                        CreatedOn = DateTime.UtcNow

                    };

                    await _unitOfWork.Meals.AddAsync(meal);
                    _unitOfWork.Complete();
                }

                // 🔹 Assign food to the meal
                var assignment = new AssignFood
                {
                    ClientId = clientId,
                    CoachingPackageRequestId = request!.Id,
                    FoodId = food.FoodId,
                    Quantity = food.Quantity,
                    Notes = food.Notes,
                    AssignedOn = DateTime.UtcNow,
                    CreatedOn = DateTime.UtcNow,
                    MealId = meal.Id,
                    MealNumber = food.MealNumber,
                    NumberOfServings = food.NumberOfServings,
                    SelectedDays = food.SelectedDays ?? new List<int>(),

                };

                await _unitOfWork.AssignFoods.AddAsync(assignment);
            }

            _unitOfWork.Complete();
        }

        public async Task AssignFoodsToFreePackageAsync(int packageId, List<AssignFoodDto> foods)
        {
            var request = _unitOfWork.CoachingPackageRequests
                .GetQueryable()
                .FirstOrDefault(r => r.PackageId == packageId && r.ClientId == null);

            if (request == null)
            {
                request = new CoachingPackageRequest
                {
                    PackageId = packageId,
                    Titles = (await _unitOfWork.CoachingPackages.GetByIdAsync(packageId))?.Title,
                    Status = ClientStatus.Active,
                    CreatedOn = DateTime.UtcNow
                };

                await _unitOfWork.CoachingPackageRequests.AddAsync(request);
                _unitOfWork.Complete();
            }

            foreach (var dto in foods)
            {
                var assignment = new AssignFood
                {
                    CoachingPackageRequestId = request.Id,
                    FoodId = dto.FoodId,   // ✅ reference existing Food
                    Quantity = dto.Quantity,
                    NumberOfServings = dto.NumberOfServings,
                    DayOfWeek = dto.DayOfWeek,
                    MealNumber = dto.MealNumber,
                    Notes = dto.Notes
                };

                await _unitOfWork.AssignFoods.AddAsync(assignment);
            }

            _unitOfWork.Complete();
        }

        public async Task DeleteAssignedExerciseAsync(int assignmentId)
        {
            var assignment = await _unitOfWork.AssignExercises.GetByIdAsync(assignmentId);
            if (assignment != null)
            {
                _unitOfWork.AssignExercises.Remove(assignment);
                _unitOfWork.Complete();
            }
        }

        public async Task DeleteAssignedFoodAsync(int assignmentId)
        {
            var assignment = await _unitOfWork.AssignFoods.GetByIdAsync(assignmentId);
            if (assignment != null)
            {
                _unitOfWork.AssignFoods.Remove(assignment);
                _unitOfWork.Complete();
            }
        }

        public async Task<List<AssignExercise>> GetAssignedExercisesAsync(int clientId)
        {
            var entities = await _unitOfWork.AssignExercises
                .GetQueryable()
                .Include(a => a.Exercise)
                .Where(a => a.ClientId == clientId && !a.IsDeleted)
                .ToListAsync();


            foreach (var e in entities)
            {
                // ensure SelectedDays is populated from DB
                if (e.SelectedDays == null || !e.SelectedDays.Any())
                {
                    e.SelectedDays = new List<int> { (int)e.DayOfWeek };
                }
            }

            return _mapper.Map<List<AssignExercise>>(entities);
        }

        public async Task<List<AssignFood>> GetAssignedFoodsAsync(int clientId)
        {
            var entities = await _unitOfWork.AssignFoods
                .GetQueryable()
                .Include(f => f.Food)
                .Include(f => f.Meal)
                .Where(f => f.ClientId == clientId && !f.IsDeleted)
                .ToListAsync();

            foreach (var e in entities)
            {
                // ensure SelectedDays is populated from DB
                if (e.SelectedDays == null || !e.SelectedDays.Any())
                {
                    e.SelectedDays = new List<int> { (int)e.DayOfWeek };
                }
            }
            return _mapper.Map<List<AssignFood>>(entities);
        }

    }
}
