namespace OnlineCoaching.Application.Services
{
    public class FoodServices : IFoodServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FoodServices(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<FoodDto> GetFoodSubstitutes(int foodId, int grams)
        {
            var selectedFood = _unitOfWork.Foods.GetById(foodId);
            if (selectedFood == null || selectedFood.IsDeleted) return Enumerable.Empty<FoodDto>();

            // calculate protein for given grams
            double selectedProtein = (selectedFood.Protein / (double)selectedFood.Gram) * grams;

            var foods = _unitOfWork.Foods.GetAll().Where(f => !f.IsDeleted && f.Id != foodId);

            // find foods that have nearly same protein content for same grams
            var substitutes = foods.Where(f =>
            {
                double proteinForGrams = (f.Protein / (double)f.Gram) * grams;
                return Math.Abs(proteinForGrams - selectedProtein) <= 1; 
            });

            return _mapper.Map<IEnumerable<FoodDto>>(substitutes);
        }

        public IEnumerable<FoodDto> GetFoodsAsync()
        {
            var foods =  _unitOfWork.Foods.GetAll().Where(f=>f.IsDeleted is false);
            return _mapper.Map<IEnumerable<FoodDto>>(foods);
        }

        public async Task<FoodDto?> GetFoodByIdAsync(int id)
        {
            var food = await  _unitOfWork.Foods.GetByIdAsync(id);

            if (food == null || food.IsDeleted) return null;
            return _mapper.Map<FoodDto>(food);
        }

        public async Task<CreateFoodDto> AddFoodAsync(Food food)
        {
            if (string.IsNullOrWhiteSpace(food.Name))
                throw new ValidationException("Food name is required.");

            food.CreatedOn = DateTime.UtcNow;
            var Addedfood = await _unitOfWork.Foods.AddAsync(food);
             _unitOfWork.Complete();
            return _mapper.Map<CreateFoodDto>(Addedfood);
        }

        public FoodDto? UpdateFood(FoodDto food)
        {
            var existing = _unitOfWork.Foods.GetById(food.Id);
            if (existing == null || existing.IsDeleted) return null;

            _mapper.Map(food, existing);

            existing.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Foods.Update(existing);
            _unitOfWork.Complete();
            return _mapper.Map<FoodDto>(existing);
        }

        public void DeleteFood(int id)
        {
            var food = _unitOfWork.Foods.GetById(id);
            if (food == null || food.IsDeleted) return;

            food.IsDeleted = true;
            food.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Foods.Update(food);
            _unitOfWork.Complete();
        }
    }
}
