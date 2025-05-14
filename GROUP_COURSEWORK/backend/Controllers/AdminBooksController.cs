using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers
{
    [Route("api/admin/books")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        public AdminBooksController(IBookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _context = context;
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

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdBook = await _bookService.CreateBookAsync(bookDto);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(id, bookDto);
                
                if (updatedBook == null)
                {
                    return NotFound(new { message = "Book not found" });
                }
                
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id, [FromQuery] bool force = false)
        {
            try
            {
                Console.WriteLine($"AdminBooksController: Deleting book {id}, force={force}");
                
                // First, check if the book exists and is not already deleted
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    Console.WriteLine($"AdminBooksController: Book {id} not found in database");
                    return NotFound(new { message = $"Book with ID {id} not found" });
                }
                
                if (book.Title.StartsWith("[DELETED]") && !force)
                {
                    Console.WriteLine($"AdminBooksController: Book {id} is already marked as deleted");
                    return Ok(new { message = $"Book with ID {id} is already deleted" });
                }
                
                var result = await _bookService.DeleteBookAsync(id, force);
                
                if (!result)
                {
                    Console.WriteLine($"AdminBooksController: Book {id} delete operation returned false");
                    return NotFound(new { message = $"Book with ID {id} could not be deleted" });
                }
                
                return Ok(new { message = $"Book with ID {id} deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AdminBooksController: Error deleting book {id}: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateBookStock(int id, [FromBody] UpdateStockDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedBook = await _bookService.UpdateBookStockAsync(id, stockDto);
                
                if (updatedBook == null)
                {
                    return NotFound(new { message = "Book not found" });
                }
                
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("discounts")]
        public async Task<IActionResult> SetDiscount([FromBody] SetDiscountDto discountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedBook = await _bookService.SetDiscountAsync(discountDto);
            
                if (updatedBook == null)
                {
                    return NotFound(new { message = "Book not found" });
                }
            
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
