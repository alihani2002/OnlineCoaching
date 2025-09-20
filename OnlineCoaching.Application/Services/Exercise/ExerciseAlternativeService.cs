namespace OnlineCoaching.Application.Services
{
    public class ExerciseAlternativeService : IExerciseAlternativeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExerciseAlternativeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ExerciseProfileDto?> GetExerciseProfileAsync(int exerciseId)
        {
            var exercise = await _unitOfWork.Exercises.GetQueryable()
                .Include(e => e.Alternatives)
                    .ThenInclude(a => a.AlternativeExercise)
                .FirstOrDefaultAsync(e => e.Id == exerciseId && !e.IsDeleted);

            if (exercise == null) return null;

            return new ExerciseProfileDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                ImageUrl = exercise.ImageUrl,
                VideoUrl = exercise.VideoUrl,
                Alternatives = exercise.Alternatives
                    .Select(a => new ExerciseAlternativeDto
                    {
                        ExerciseId = a.ExerciseId,
                        AlternativeExerciseId = a.AlternativeExerciseId,
                        AlternativeExerciseName = a.AlternativeExercise?.Name
                    }).ToList()
            };
        }

        public async Task<IEnumerable<Exercise>> GetAllExercisesWithAlternativesAsync()
        {
            return await _unitOfWork.Exercises.GetQueryable()
        .Include(e => e.Alternatives)
            .ThenInclude(a => a.AlternativeExercise)
        .Where(e => !e.IsDeleted && e.Alternatives.Any()) // ✅ only exercises with alternatives
        .ToListAsync();
        }

        // ✅ جلب كل البدائل لتمرين معين
        public async Task<IEnumerable<ExerciseAlternativeDto>> GetAlternativesByExerciseIdAsync(int exerciseId)
        {
            var alternatives = await _unitOfWork.ExerciseAlternatives.GetQueryable()
                .Include(ea => ea.AlternativeExercise)
                .Include(ea => ea.Exercise)
                .Where(ea => ea.ExerciseId == exerciseId)
                .ToListAsync();

            return alternatives.Select(a => new ExerciseAlternativeDto
            {
                ExerciseId = a.ExerciseId,
                AlternativeExerciseId = a.AlternativeExerciseId,
                ExerciseName = a.Exercise?.Name,
                AlternativeExerciseName = a.AlternativeExercise?.Name
            });
        }

        // ✅ إضافة بديل جديد
        public async Task<ExerciseAlternativeDto?> AddAlternativeAsync(CreateExerciseAlternativeDto dto)
        {
            if (dto.ExerciseId == dto.AlternativeExerciseId)
                throw new ArgumentException("لا يمكن تعيين نفس التمرين كبديل لنفسه.");

            // Check if already exists
            var exists = await _unitOfWork.ExerciseAlternatives.GetQueryable()
                .AnyAsync(ea => ea.ExerciseId == dto.ExerciseId && ea.AlternativeExerciseId == dto.AlternativeExerciseId);

            if (exists)
                throw new InvalidOperationException("هذا البديل مسجل بالفعل.");

            var entity = new ExerciseAlternative
            {
                ExerciseId = dto.ExerciseId,
                AlternativeExerciseId = dto.AlternativeExerciseId
            };

            await _unitOfWork.ExerciseAlternatives.AddAsync(entity);
            _unitOfWork.Complete();

            return new ExerciseAlternativeDto
            {
                ExerciseId = entity.ExerciseId,
                AlternativeExerciseId = entity.AlternativeExerciseId
            };
        }

        // ✅ حذف بديل
        public async Task<bool> DeleteAlternativeAsync(int exerciseId, int alternativeExerciseId)
        {
            var entity = await _unitOfWork.ExerciseAlternatives.GetQueryable()
                .FirstOrDefaultAsync(ea => ea.ExerciseId == exerciseId && ea.AlternativeExerciseId == alternativeExerciseId);

            if (entity == null) return false;

            _unitOfWork.ExerciseAlternatives.Remove(entity);
            _unitOfWork.Complete();
            return true;
        }
    }
}
