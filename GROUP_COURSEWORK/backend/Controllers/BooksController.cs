using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] BookQueryParametersDto parameters)
        {
            var paginatedBooks = await _bookService.GetPaginatedBooksAsync(parameters);
            return Ok(paginatedBooks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var bookDto = await _bookService.GetBookByIdAsync(id);
            
            if (bookDto == null)
            {
                return NotFound(new { message = "Book not found" });
            }
            
            return Ok(bookDto);
        }

        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _bookService.GetUniqueGenresAsync();
            return Ok(genres);
        }

        [HttpGet("authors")]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _bookService.GetUniqueAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("publishers")]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _bookService.GetUniquePublishersAsync();
            return Ok(publishers);
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _bookService.GetUniqueLanguagesAsync();
            return Ok(languages);
        }

        [HttpGet("formats")]
        public async Task<IActionResult> GetFormats()
        {
            var formats = await _bookService.GetUniqueFormatsAsync();
            return Ok(formats);
        }
    }
}
