namespace OnlineCoaching.Application.Services
{
    public interface IFoodServices
    {
        IEnumerable<FoodDto> GetFoodsAsync();
        Task<FoodDto?> GetFoodByIdAsync(int id);
        Task<CreateFoodDto> AddFoodAsync(Food food);
        FoodDto? UpdateFood(FoodDto food);
        void DeleteFood(int id);
        IEnumerable<FoodDto> GetFoodSubstitutes(int foodId, int grams);

    }
}
