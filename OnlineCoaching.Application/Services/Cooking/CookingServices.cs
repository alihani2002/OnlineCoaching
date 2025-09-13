using OnlineCoaching.Domain.Dtos.Cooking;

namespace OnlineCoaching.Application.Services
{
    public class CookingServices : ICookingServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CookingServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<Cooking> GetCookings()
        {
            var entity = _unitOfWork.Cookings.GetQueryable().Where(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<Cooking>>(entity);
        }

        public Cookingdto GetCookingById(int id)
        {
            var entity = _unitOfWork.Cookings.GetById(id);
            return _mapper.Map<Cookingdto>(entity);
        }

        public async Task<Cookingdto> Create(Cookingdto cookingdto)
        {
            var cooking = _mapper.Map<Cooking>(cookingdto);
            await _unitOfWork.Cookings.AddAsync(cooking);
            _unitOfWork.Complete();
            return _mapper.Map<Cookingdto>(cooking);
        }

        public async Task<Cookingdto?> Update(int id, Cookingdto cookingdto, string? updatedById = null)
        {
            var entity = await _unitOfWork.Cookings.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            // map new values
            _mapper.Map(cookingdto, entity);

            entity.LastUpdatedOn = DateTime.UtcNow;
            entity.LastUpdatedById = updatedById;

            _unitOfWork.Cookings.Update(entity);
            _unitOfWork.Complete();

            return _mapper.Map<Cookingdto>(entity);
        }

        public async Task<bool> DeleteCookingAsync(int id)
        {
            var entity = await _unitOfWork.Cookings.GetByIdAsync(id);

            if (entity == null || entity.IsDeleted)
                return false;

            entity.IsDeleted = true;
            entity.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Cookings.Update(entity);
            _unitOfWork.Complete();

            return true;
        }

    }
}
