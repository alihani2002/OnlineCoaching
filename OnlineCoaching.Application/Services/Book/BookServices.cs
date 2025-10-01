using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public class BookServices : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Book CRUD

        public IEnumerable<BookDto> GetBooks()
        {
            var books = _unitOfWork.Books.GetQueryable()
                         .Where(b => !b.IsDeleted)
                         .ToList();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null || book.IsDeleted) return null;

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> AddBookAsync(CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ValidationException("Book title is required.");

            var book = _mapper.Map<Book>(dto);
            book.CreatedOn = DateTime.UtcNow;

            var addedBook = await _unitOfWork.Books.AddAsync(book);
            _unitOfWork.Complete();

            return _mapper.Map<BookDto>(addedBook);
        }

        public async Task<BookDto?> UpdateBookAsync(BookDto dto)
        {
            var existing = await _unitOfWork.Books.GetByIdAsync(dto.Id);
            if (existing == null || existing.IsDeleted) return null;

            _mapper.Map(dto, existing);
            existing.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Books.Update(existing);
            _unitOfWork.Complete();

            return _mapper.Map<BookDto>(existing);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null || book.IsDeleted) return false;

            book.IsDeleted = true;
            book.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.Books.Update(book);
            _unitOfWork.Complete();

            return true;
        }

        #endregion

        #region Book Requests

        public async Task<BookRequestDto> AddBookRequestAsync(CreateBookRequestDto dto)
        {
            var bookRequest = _mapper.Map<BookRequest>(dto);
            bookRequest.CreatedOn = DateTime.UtcNow;

            var addedRequest = await _unitOfWork.BookRequests.AddAsync(bookRequest);
            _unitOfWork.Complete();

            return _mapper.Map<BookRequestDto>(addedRequest);
        }

        public async Task<IEnumerable<BookRequestDto>> GetBookRequestsAsync()
        {
            var requests = await _unitOfWork.BookRequests.GetQueryable()
                              .Include(r => r.Book)
                              .Include(r => r.Client)
                              .Where(r => !r.IsDeleted)
                              .OrderByDescending(r => r.CreatedOn)
                              .ToListAsync();

            return _mapper.Map<IEnumerable<BookRequestDto>>(requests);
        }

        public async Task<bool> ApproveRequestAsync(int id)
        {
            var request = await _unitOfWork.BookRequests.GetByIdAsync(id);
            if (request == null || request.IsDeleted) return false;

            request.IsApproved = true;
            request.Status = ClientStatus.Active;
            request.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.BookRequests.Update(request);
            _unitOfWork.Complete();

            return true;
        }

        public async Task ToggleDeleteAsync(int requestId)
        {
            var request = await _unitOfWork.BookRequests.GetByIdAsync(requestId);
            if (request == null)
                throw new Exception("Request not found");

            request.IsDeleted = !request.IsDeleted; // toggle
            _unitOfWork.BookRequests.Update(request);
            _unitOfWork.Complete();
        }
        #endregion
    }
}
