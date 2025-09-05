using OnlineCoaching.Domain.Dtos.ExerciseSheet;

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
            var exercises = _unitOfWork.Exercises.GetQueryable().Include(e => e.Muscle)
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

        public async Task<IEnumerable<ExerciseSheetLog>> GetAllLogs()
        {
            var logs = await _unitOfWork.ExerciseSheetLogs.GetQueryable()
                .Include(c => c.Client)
                .Include(e => e.Exercise)
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExerciseSheetLog>>(logs);
        }

        public async Task<IEnumerable<ExerciseSheetLog>> GetLogsByClientAndExercise(int clientId, int exerciseId)
        {
            var logs = await _unitOfWork.ExerciseSheetLogs.GetQueryable()
                .Include(c => c.Client)
                .Include(e => e.Exercise)
                .Where(l => l.ClientId == clientId && l.ExerciseId == exerciseId  && !l.IsDeleted ) 
                .OrderByDescending(l => l.CreatedOn)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExerciseSheetLog>>(logs);
        }

        public async Task<ExerciseSheetLog> AddExerciseSheetLog(CreateExerciseSheetDto dto)
        {
            if (dto == null)
                throw new ValidationException("Exercise log data is required.");

            var log = _mapper.Map<ExerciseSheetLog>(dto);
            log.CreatedOn = DateTime.Now;

            var addedLog = await _unitOfWork.ExerciseSheetLogs.AddAsync(log);
            _unitOfWork.Complete();

            return _mapper.Map<ExerciseSheetLog>(addedLog);
        }

        public async Task<ExerciseSheetLog?> UpdateExerciseSheetLog(ExerciseSheetLog dto)
        {
            var existingLog = await _unitOfWork.ExerciseSheetLogs.GetByIdAsync(dto.Id);
            if (existingLog == null || existingLog.IsDeleted) return null;

            _mapper.Map(dto, existingLog);
            existingLog.LastUpdatedOn = DateTime.Now;

            _unitOfWork.ExerciseSheetLogs.Update(existingLog);
            _unitOfWork.Complete();

            return _mapper.Map<ExerciseSheetLog>(existingLog);
        }

        public async Task<bool> DeletedLogs(int id)
        {
            var log = await _unitOfWork.ExerciseSheetLogs.GetByIdAsync(id);
            if (log == null) return false;
            log.IsDeleted = !log.IsDeleted;
            log.LastUpdatedOn = DateTime.Now;
            _unitOfWork.ExerciseSheetLogs.Update(log);
            _unitOfWork.Complete();
            return true;
        }

    }
}