using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.Domain.Entities.Foods;

namespace OnlineCoaching.Application.Services.AssignmentService
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICoachingPackageRequestService _coachingPackageRequestService;
        public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper , ICoachingPackageRequestService coachingPackageRequestService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _coachingPackageRequestService = coachingPackageRequestService;
        }

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
                    AssignedOn = DateTime.UtcNow ,
                    CreatedOn = DateTime.UtcNow,
                    DayOfWeek = ex.DayOfWeek
                };

                await _unitOfWork.AssignExercises.AddAsync(assignment);
            }

            _unitOfWork.Complete();
        }

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
                    MealId = meal.Id ,
                    MealNumber = food.MealNumber,
                    NumberOfServings = food.NumberOfServings,

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



    }
}
