using OnlineCoaching.Domain.Dtos.Cooking;

namespace OnlineCoaching.Application.Services
{
    public interface ICookingServices
    {
        IEnumerable<Cooking> GetCookings();
        Cookingdto GetCookingById(int id);
        Task<Cookingdto> Create(Cookingdto cookingdto);
        Task<Cookingdto?> Update(int id, Cookingdto cookingdto, string? updatedById = null);
        Task<bool> DeleteCookingAsync(int id);
    }
}
