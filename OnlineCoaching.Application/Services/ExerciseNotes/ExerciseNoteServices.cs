using OnlineCoaching.Domain.Dtos.Notes;

namespace OnlineCoaching.Application.Services
{
    public class ExerciseNoteServices : IExerciseNoteServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExerciseNoteServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ExerciseNoteDto> AddAsync(CreateExerciseNoteDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Note))
                throw new ValidationException("Note cannot be empty.");

            // ✅ Check exercise existence
            var exerciseExists = await _unitOfWork.Exercises
                .GetQueryable()
                .AnyAsync(e => e.Id == dto.ExerciseId);

            if (!exerciseExists)
                throw new ValidationException("Invalid ExerciseId. Exercise does not exist.");

            var note = _mapper.Map<ExerciseNotes>(dto);
            note.CreatedOn = DateTime.UtcNow;

            var addedNote = await _unitOfWork.ExerciseNotes.AddAsync(note);
            _unitOfWork.Complete();

            return _mapper.Map<ExerciseNoteDto>(addedNote);
        }


        // ✅ Get Note by Id
        public async Task<ExerciseNoteDto?> GetByIdAsync(int id)
        {
            var note = await _unitOfWork.ExerciseNotes
                .GetQueryable()
                .Include(n => n.Client)
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);

            return note == null ? null : _mapper.Map<ExerciseNoteDto>(note);
        }

        // ✅ Get All Notes for a Specific Exercise
        public async Task<IEnumerable<ExerciseNoteDto>> GetNotesByExerciseAsync(int exerciseId)
        {
            var notes = await _unitOfWork.ExerciseNotes
                .GetQueryable()
                .Include(n => n.Client)
                .Where(n => n.ExerciseId == exerciseId && !n.IsDeleted)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ExerciseNoteDto>>(notes);
        }

        // ✅ Delete Note (Soft Delete)
        public async Task<bool> DeleteAsync(int id)
        {
            var note = await _unitOfWork.ExerciseNotes.GetByIdAsync(id);
            if (note == null || note.IsDeleted) return false;

            note.IsDeleted = true;
            note.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.ExerciseNotes.Update(note);
            _unitOfWork.Complete();

            return true;
        }

        // ✅ Get All Notes with Exercise + Client Name
        public async Task<IEnumerable<ExerciseNoteDto>> GetAllWithDetailsAsync()
        {
            var notes = await _unitOfWork.ExerciseNotes
                .GetQueryable()
                .Include(n => n.Client)
                .Include(n => n.Exercise)
                .Where(n => !n.IsDeleted)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();

            return notes.Select(n => new ExerciseNoteDto
            {
                Id = n.Id,
                Note = n.Note ?? string.Empty,
                ExerciseId = n.ExerciseId ?? 0,
                ExerciseName = n.Exercise?.Name,  // ✅ Exercise name
                ClientId = n.ClientId,
                ClientName = n.Client?.FullName,            // ✅ Client name
                CreatedOn = n.CreatedOn
            }).ToList();
        }

    }
}
