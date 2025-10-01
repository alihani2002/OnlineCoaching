namespace OnlineCoaching.Application.Services
{
    public interface IBookService
    {
        // 📚 Book CRUD
        IEnumerable<BookDto> GetBooks();
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto> AddBookAsync(CreateBookDto dto);
        Task<BookDto?> UpdateBookAsync(BookDto dto);
        Task<bool> DeleteBookAsync(int id);

        // 📖 Book Requests
        Task<BookRequestDto> AddBookRequestAsync(CreateBookRequestDto dto);
        Task<IEnumerable<BookRequestDto>> GetBookRequestsAsync();
        Task<bool> ApproveRequestAsync(int id);
        Task ToggleDeleteAsync(int requestId);
    }
}
