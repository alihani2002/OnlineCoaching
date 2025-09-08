namespace OnlineCoaching.Application.Services
{
    public interface ITransformationService
    {
        Task<Transformation> CreateAsync(Transformation transformation);
        Transformation? GetById(int id);
        IEnumerable<Transformation> GetAll();
        Transformation Update(Transformation transformation);
        Task<bool> DeleteAsync(int id);
    }
}
