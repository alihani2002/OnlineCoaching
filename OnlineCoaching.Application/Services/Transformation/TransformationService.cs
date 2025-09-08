namespace OnlineCoaching.Application.Services
{
    public class TransformationService : ITransformationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransformationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Transformation> CreateAsync(Transformation transformation)
        {
            await _unitOfWork.Transformations.AddAsync(transformation);
            _unitOfWork.Complete();
            return transformation;
        }

        public Transformation? GetById(int id)
        {
            return _unitOfWork.Transformations.GetQueryable().Include(t => t.Client).Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Transformation> GetAll()
        {
            return _unitOfWork.Transformations.GetQueryable().Include(t=> t.Client).Where(t=> t.IsDeleted != true).ToList();
        }

        public Transformation Update(Transformation transformation)
        {
            _unitOfWork.Transformations.Update(transformation);
            _unitOfWork.Complete();
            return transformation;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Transformations.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            _unitOfWork.Transformations.Update(entity);
            _unitOfWork.Complete();
            return true;
        }
    }
}
