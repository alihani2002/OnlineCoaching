namespace OnlineCoaching.Application.Services
{
    public class ExerciseServices : IExerciseServices
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public ExerciseServices(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public IEnumerable<ExerciseDto> GetExercises()
            {
                var exercises = _unitOfWork.Exercises.GetAll()
                                    .Where(e => !e.IsDeleted)
                                    .ToList();
                return _mapper.Map<IEnumerable<ExerciseDto>>(exercises);
            }

            public async Task<ExerciseDto?> GetExerciseByIdAsync(int id)
            {
                var exercise = await _unitOfWork.Exercises.GetByIdAsync(id);
                if (exercise == null || exercise.IsDeleted) return null;

                return _mapper.Map<ExerciseDto>(exercise);
            }

            public async Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto dto)
            {
                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ValidationException("Exercise name is required.");

                var exercise = _mapper.Map<Exercise>(dto);
                exercise.CreatedOn = DateTime.UtcNow;

                var addedExercise = await _unitOfWork.Exercises.AddAsync(exercise);
                _unitOfWork.Complete();

                return _mapper.Map<ExerciseDto>(addedExercise);
            }

            public async Task<ExerciseDto?> UpdateExerciseAsync(ExerciseDto dto)
            {
                var existing = await _unitOfWork.Exercises.GetByIdAsync(dto.Id);
                if (existing == null || existing.IsDeleted) return null;

                _mapper.Map(dto, existing);
                existing.LastUpdatedOn = DateTime.UtcNow;

                _unitOfWork.Exercises.Update(existing);
                _unitOfWork.Complete();

                return _mapper.Map<ExerciseDto>(existing);
            }

            public async Task<bool> DeleteExerciseAsync(int id)
            {
                var exercise = await _unitOfWork.Exercises.GetByIdAsync(id);
                if (exercise == null || exercise.IsDeleted) return false;

                exercise.IsDeleted = true;
                exercise.LastUpdatedOn = DateTime.UtcNow;

                _unitOfWork.Exercises.Update(exercise);
                _unitOfWork.Complete();

                return true;
            }
        }
    }
