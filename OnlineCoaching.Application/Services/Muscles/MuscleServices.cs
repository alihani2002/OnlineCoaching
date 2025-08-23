namespace OnlineCoaching.Application.Services
{
    public class MuscleServices : IMuscleServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MuscleServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MuscleDto> GetMuscles()
        {
            var muscles = _unitOfWork.Muscles.GetAll().Where(m => !m.IsDeleted);
            return _mapper.Map<IEnumerable<MuscleDto>>(muscles);
        }

        public async Task<MuscleDto?> GetMuscleByIdAsync(int id)
        {
            var muscle = await _unitOfWork.Muscles.GetByIdAsync(id);
            if (muscle == null || muscle.IsDeleted) return null;

            return _mapper.Map<MuscleDto>(muscle);
        }

        public async Task<MuscleDto> AddMuscleAsync(CreateMuscleDto muscleDto)
        {
            if (string.IsNullOrWhiteSpace(muscleDto.Name))
                throw new ValidationException("Muscle name is required.");

            var muscle = _mapper.Map<Muscle>(muscleDto);
            muscle.CreatedOn = DateTime.UtcNow;

            var addedMuscle = await _unitOfWork.Muscles.AddAsync(muscle);
             _unitOfWork.Complete();

            return _mapper.Map<MuscleDto>(addedMuscle);
        }

        public async Task<MuscleDto?> UpdateMuscleAsync(MuscleDto muscleDto)
        {
            var existing = await _unitOfWork.Muscles.GetByIdAsync(muscleDto.Id);
            if (existing == null || existing.IsDeleted) return null;

            _mapper.Map(muscleDto, existing);
            existing.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Muscles.Update(existing);
             _unitOfWork.Complete();

            return _mapper.Map<MuscleDto>(existing);
        }

        public async Task<bool> DeleteMuscleAsync(int id)
        {
            var muscle = await _unitOfWork.Muscles.GetByIdAsync(id);
            if (muscle == null || muscle.IsDeleted) return false;

            muscle.IsDeleted = true;
            muscle.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Muscles.Update(muscle);
             _unitOfWork.Complete();

            return true;
        }
    }
}

