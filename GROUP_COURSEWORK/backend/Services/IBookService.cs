using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IBookService
    {
        Task<PaginatedResponseDto<BookDto>> GetPaginatedBooksAsync(BookQueryParametersDto parameters);
        Task<BookDto> GetBookByIdAsync(int id);
        Task<IEnumerable<string>> GetUniqueGenresAsync();
        Task<IEnumerable<string>> GetUniqueAuthorsAsync();
        Task<IEnumerable<string>> GetUniquePublishersAsync();
        Task<IEnumerable<string>> GetUniqueLanguagesAsync();
        Task<IEnumerable<string>> GetUniqueFormatsAsync();
        
        // Admin operations
        Task<BookDto> CreateBookAsync(CreateBookDto bookDto);
        Task<BookDto> UpdateBookAsync(int id, UpdateBookDto bookDto);
        Task<bool> DeleteBookAsync(int id, bool force = false);
        Task<BookDto> UpdateBookStockAsync(int id, UpdateStockDto stockDto);

        // Discount operations
        Task<BookDto> SetDiscountAsync(SetDiscountDto discountDto);
    }
}
