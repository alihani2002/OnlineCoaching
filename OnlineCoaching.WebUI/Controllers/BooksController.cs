using Microsoft.AspNetCore.Mvc;
using OnlineCoaching.Application.Services;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.WebUI; // for IFileService

namespace OnlineCoaching.WebUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IImageService _fileService;

        public BooksController(IBookService bookService, IImageService fileService)
        {
            _bookService = bookService;
            _fileService = fileService;
        }

        // ===================== BOOKS =====================

        // GET: /Books
        // BooksController.cs

        // GET: /Books
        public IActionResult Index(string searchString)
        {
            // Pass the search string back to the view to keep it in the search box
            ViewData["CurrentFilter"] = searchString;

            var books = _bookService.GetBooks();

            // If a search string is provided, filter the list
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerCaseSearch = searchString.ToLower();
                books = books.Where(b =>
                    b.Title!.ToLower().Contains(lowerCaseSearch) ||
                    b.Author!.ToLower().Contains(lowerCaseSearch)
                ).ToList();
            }

            return View(books);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookDto dto, IFormFile? file, IFormFile? demoFile)
        {
            if (!ModelState.IsValid) return View(dto);

            if (file != null)
                dto.FileUrl = await _fileService.UploadPdfAsync(file);

            if (demoFile != null)
                dto.DemoUrl = await _fileService.UploadPdfAsync(demoFile);

            await _bookService.AddBookAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        //// GET: /Books/Edit/5
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var book = await _bookService.GetBookByIdAsync(id);
        //    if (book == null) return NotFound();

        //    return View(book);
        //}

        //// POST: /Books/Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(BookDto dto, IFormFile? file, IFormFile? demoFile)
        //{
        //    if (!ModelState.IsValid) return View(dto);

        //    if (file != null)
        //        dto.FileUrl = await _fileService.UploadPdfAsync(file);

        //    if (demoFile != null)
        //        dto.DemoUrl = await _fileService.UploadPdfAsync(demoFile);

        //    await _bookService.UpdateBookAsync(dto);
        //    return RedirectToAction(nameof(Index));
        //}

        // POST: /Books/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // ===================== BOOK REQUESTS =====================

        public async Task<IActionResult> Requests()
        {
            var requests = await _bookService.GetBookRequestsAsync();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            await _bookService.ApproveRequestAsync(id);
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDeleteRequest(int id)
        {
            await _bookService.ToggleDeleteAsync(id);
            return RedirectToAction(nameof(Requests));
        }

        //    public async Task<IActionResult> Download(int id)
        //    {
        //        var book = await _bookService.GetBookByIdAsync(id);

        //        if (book == null || string.IsNullOrEmpty(book.FileUrl))
        //        {
        //            return NotFound("Book or file not found.");
        //        }

        //        try
        //        {
        //            // Use HttpClient to fetch the file from the Cloudinary URL
        //            using (var httpClient = new HttpClient())
        //            {
        //                var fileBytes = await httpClient.GetByteArrayAsync(book.FileUrl);

        //                // Create a user-friendly filename for the download
        //                var downloadFileName = $"{book.Title!.Replace(" ", "_")}.pdf";

        //                // Return the file with the correct content type to trigger a download
        //                return File(fileBytes, "application/pdf", downloadFileName);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception (optional) and return an error
        //            return StatusCode(500, $"An error occurred while downloading the file: {ex.Message}");
        //        }
        //    }
        //}
    }


}