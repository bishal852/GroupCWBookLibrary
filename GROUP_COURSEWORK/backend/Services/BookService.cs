using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponseDto<BookDto>> GetPaginatedBooksAsync(BookQueryParametersDto parameters)
        {
            // Ensure page and size are valid
            parameters.Page = parameters.Page < 1 ? 1 : parameters.Page;
            parameters.Size = parameters.Size < 1 ? 10 : parameters.Size;

            // Start with all books that are not deleted
            IQueryable<Book> booksQuery = _context.Books.Where(b => !b.Title.StartsWith("[DELETED]"));

            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                string searchTerm = parameters.Search.ToLower();
                booksQuery = booksQuery.Where(b => 
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Author.ToLower().Contains(searchTerm) ||
                    b.Description.ToLower().Contains(searchTerm) ||
                    b.ISBN.ToLower().Contains(searchTerm) ||
                    b.Publisher.ToLower().Contains(searchTerm)
                );
            }

            // Apply filters
            if (!string.IsNullOrWhiteSpace(parameters.Genre))
            {
                booksQuery = booksQuery.Where(b => b.Genre == parameters.Genre);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Author))
            {
                booksQuery = booksQuery.Where(b => b.Author == parameters.Author);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Publisher))
            {
                booksQuery = booksQuery.Where(b => b.Publisher == parameters.Publisher);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Language))
            {
                booksQuery = booksQuery.Where(b => b.Language == parameters.Language);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Format))
            {
                booksQuery = booksQuery.Where(b => b.Format == parameters.Format);
            }

            if (parameters.MinPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price <= parameters.MaxPrice.Value);
            }

            if (parameters.OnSaleOnly == true)
            {
                booksQuery = booksQuery.Where(b => b.IsOnSale);
            }

            if (parameters.InStockOnly == true)
            {
                booksQuery = booksQuery.Where(b => b.StockQuantity > 0);
            }

            // Get total count after filtering
            var totalItems = await booksQuery.CountAsync();
            
            // Apply sorting
            booksQuery = ApplySorting(booksQuery, parameters.SortBy, parameters.SortDescending);
            
            // Apply pagination
            var books = await booksQuery
                .Skip((parameters.Page - 1) * parameters.Size)
                .Take(parameters.Size)
                .ToListAsync();
            
            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.Size);
            
            // Map to DTOs
            var bookDtos = books.Select(MapBookToDto).ToList();
            
            // Create and return paginated response
            return new PaginatedResponseDto<BookDto>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = parameters.Page,
                PageSize = parameters.Size,
                Items = bookDtos
            };
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null || book.Title.StartsWith("[DELETED]"))
            {
                return null;
            }
            
            return MapBookToDto(book);
        }

        public async Task<IEnumerable<string>> GetUniqueGenresAsync()
        {
            return await _context.Books
                .Where(b => !b.Title.StartsWith("[DELETED]"))
                .Select(b => b.Genre)
                .Distinct()
                .Where(g => !string.IsNullOrEmpty(g))
                .OrderBy(g => g)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUniqueAuthorsAsync()
        {
            return await _context.Books
                .Where(b => !b.Title.StartsWith("[DELETED]"))
                .Select(b => b.Author)
                .Distinct()
                .Where(a => !string.IsNullOrEmpty(a))
                .OrderBy(a => a)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUniquePublishersAsync()
        {
            return await _context.Books
                .Where(b => !b.Title.StartsWith("[DELETED]"))
                .Select(b => b.Publisher)
                .Distinct()
                .Where(p => !string.IsNullOrEmpty(p))
                .OrderBy(p => p)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUniqueLanguagesAsync()
        {
            return await _context.Books
                .Where(b => !b.Title.StartsWith("[DELETED]"))
                .Select(b => b.Language)
                .Distinct()
                .Where(l => !string.IsNullOrEmpty(l))
                .OrderBy(l => l)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUniqueFormatsAsync()
        {
            return await _context.Books
                .Where(b => !b.Title.StartsWith("[DELETED]"))
                .Select(b => b.Format)
                .Distinct()
                .Where(f => !string.IsNullOrEmpty(f))
                .OrderBy(f => f)
                .ToListAsync();
        }
        
        // Admin operations
        public async Task<BookDto> CreateBookAsync(CreateBookDto bookDto)
        {
            // Check if ISBN already exists
            if (await _context.Books.AnyAsync(b => b.ISBN == bookDto.ISBN && !b.Title.StartsWith("[DELETED]")))
            {
                throw new Exception("A book with this ISBN already exists");
            }

            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                ISBN = bookDto.ISBN,
                Description = bookDto.Description,
                Price = bookDto.Price,
                Genre = bookDto.Genre,
                Language = bookDto.Language,
                Format = bookDto.Format,
                Publisher = bookDto.Publisher,
                PublicationDate = bookDto.PublicationDate,
                StockQuantity = bookDto.StockQuantity,
                CoverImageUrl = bookDto.CoverImageUrl,
                AverageRating = 0, // New books start with no ratings
                IsOnSale = bookDto.IsOnSale,
                DiscountPrice = bookDto.DiscountPrice,
                DiscountStartDate = bookDto.DiscountStartDate,
                DiscountEndDate = bookDto.DiscountEndDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return MapBookToDto(book);
        }

        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto bookDto)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null || book.Title.StartsWith("[DELETED]"))
            {
                return null;
            }

            // Check if ISBN is being changed and if the new ISBN already exists
            if (book.ISBN != bookDto.ISBN && await _context.Books.AnyAsync(b => b.ISBN == bookDto.ISBN && !b.Title.StartsWith("[DELETED]")))
            {
                throw new Exception("A book with this ISBN already exists");
            }

            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.ISBN = bookDto.ISBN;
            book.Description = bookDto.Description;
            book.Price = bookDto.Price;
            book.Genre = bookDto.Genre;
            book.Language = bookDto.Language;
            book.Format = bookDto.Format;
            book.Publisher = bookDto.Publisher;
            book.PublicationDate = bookDto.PublicationDate;
            book.StockQuantity = bookDto.StockQuantity;
            book.CoverImageUrl = bookDto.CoverImageUrl;
            book.IsOnSale = bookDto.IsOnSale;
            book.DiscountPrice = bookDto.DiscountPrice;
            book.DiscountStartDate = bookDto.DiscountStartDate;
            book.DiscountEndDate = bookDto.DiscountEndDate;
            book.UpdatedAt = DateTime.UtcNow;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return MapBookToDto(book);
        }

        public async Task<bool> DeleteBookAsync(int id, bool force = false)
        {
            Console.WriteLine($"Attempting to delete book {id}, force={force}");
            
            var book = await _context.Books.FindAsync(id);
            
            if (book == null)
            {
                Console.WriteLine($"Book {id} not found");
                return false;
            }

            // Check if the book is referenced in any order
            bool isInOrder = await _context.OrderItems.AnyAsync(oi => oi.BookId == id);
            
            if (isInOrder)
            {
                Console.WriteLine($"Book {id} is referenced in orders");
                
                // If the book is in orders, we can't physically delete it
                // Instead, we'll mark it as deleted and make it unavailable
                book.StockQuantity = 0;
                book.IsOnSale = false;
                book.Title = $"[DELETED] {book.Title}";
                book.UpdatedAt = DateTime.UtcNow;
                
                _context.Books.Update(book);
                
                // Remove from wishlists and carts
                var wishlists = await _context.Wishlists.Where(w => w.BookId == id).ToListAsync();
                if (wishlists.Any())
                {
                    Console.WriteLine($"Removing {wishlists.Count} wishlist entries for book {id}");
                    _context.Wishlists.RemoveRange(wishlists);
                }

                var cartItems = await _context.Carts.Where(c => c.BookId == id).ToListAsync();
                if (cartItems.Any())
                {
                    Console.WriteLine($"Removing {cartItems.Count} cart items for book {id}");
                    _context.Carts.RemoveRange(cartItems);
                }
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"Book {id} marked as deleted");
                return true;
            }
            
            // If the book is not in orders, we can check other references
            if (!force)
            {
                // Check if the book is in any wishlist
                bool isInWishlist = await _context.Wishlists.AnyAsync(w => w.BookId == id);
                if (isInWishlist)
                {
                    Console.WriteLine($"Book {id} is in wishlists");
                    throw new Exception("Cannot delete book because it is in one or more wishlists. Use force=true to delete anyway.");
                }

                // Check if the book is in any cart
                bool isInCart = await _context.Carts.AnyAsync(c => c.BookId == id);
                if (isInCart)
                {
                    Console.WriteLine($"Book {id} is in carts");
                    throw new Exception("Cannot delete book because it is in one or more carts. Use force=true to delete anyway.");
                }

                // Check if the book has any reviews
                bool hasReviews = await _context.Reviews.AnyAsync(r => r.BookId == id);
                if (hasReviews)
                {
                    Console.WriteLine($"Book {id} has reviews");
                    throw new Exception("Cannot delete book because it has one or more reviews. Use force=true to delete anyway.");
                }
            }
            else
            {
                // If force delete is enabled, remove all references first
                var wishlists = await _context.Wishlists.Where(w => w.BookId == id).ToListAsync();
                if (wishlists.Any())
                {
                    Console.WriteLine($"Force removing {wishlists.Count} wishlist entries for book {id}");
                    _context.Wishlists.RemoveRange(wishlists);
                }

                var cartItems = await _context.Carts.Where(c => c.BookId == id).ToListAsync();
                if (cartItems.Any())
                {
                    Console.WriteLine($"Force removing {cartItems.Count} cart items for book {id}");
                    _context.Carts.RemoveRange(cartItems);
                }

                var reviews = await _context.Reviews.Where(r => r.BookId == id).ToListAsync();
                if (reviews.Any())
                {
                    Console.WriteLine($"Force removing {reviews.Count} reviews for book {id}");
                    _context.Reviews.RemoveRange(reviews);
                }
            }

            // Now we can safely delete the book
            Console.WriteLine($"Physically deleting book {id}");
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Book {id} deleted successfully");

            return true;
        }

        public async Task<BookDto> UpdateBookStockAsync(int id, UpdateStockDto stockDto)
        {
            var book = await _context.Books.FindAsync(id);
            
            if (book == null || book.Title.StartsWith("[DELETED]"))
            {
                return null;
            }

            book.StockQuantity = stockDto.StockQuantity;
            book.UpdatedAt = DateTime.UtcNow;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return MapBookToDto(book);
        }
        
        public async Task<BookDto> SetDiscountAsync(SetDiscountDto discountDto)
        {
            var book = await _context.Books.FindAsync(discountDto.BookId);
            
            if (book == null || book.Title.StartsWith("[DELETED]"))
            {
                return null;
            }

            // Calculate the discount price based on the percentage
            decimal discountedPrice = book.Price - (book.Price * discountDto.DiscountPercent / 100);
            
            // Round to 2 decimal places
            discountedPrice = Math.Round(discountedPrice, 2);

            // Update the book with discount information
            book.IsOnSale = discountDto.OnSale;
            book.DiscountPrice = discountedPrice;
            book.DiscountStartDate = discountDto.StartDate;
            book.DiscountEndDate = discountDto.EndDate;
            book.UpdatedAt = DateTime.UtcNow;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return MapBookToDto(book);
        }
        
        private IQueryable<Book> ApplySorting(IQueryable<Book> query, string sortBy, bool descending)
        {
            switch (sortBy.ToLower())
            {
                case "title":
                    return descending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title);
                case "author":
                    return descending ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author);
                case "price":
                    return descending ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price);
                case "rating":
                    return descending ? query.OrderByDescending(b => b.AverageRating) : query.OrderBy(b => b.AverageRating);
                case "publicationdate":
                    return descending ? query.OrderByDescending(b => b.PublicationDate) : query.OrderBy(b => b.PublicationDate);
                case "stockquantity":
                    return descending ? query.OrderByDescending(b => b.StockQuantity) : query.OrderBy(b => b.StockQuantity);
                default: // Default to CreatedAt
                    return descending ? query.OrderByDescending(b => b.CreatedAt) : query.OrderBy(b => b.CreatedAt);
            }
        }
        
        private BookDto MapBookToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Description = book.Description,
                Price = book.Price,
                Genre = book.Genre,
                Language = book.Language,
                Format = book.Format,
                Publisher = book.Publisher,
                PublicationDate = book.PublicationDate,
                StockQuantity = book.StockQuantity,
                CoverImageUrl = book.CoverImageUrl,
                AverageRating = book.AverageRating,
                IsOnSale = book.IsOnSale,
                DiscountPrice = book.DiscountPrice,
                DiscountStartDate = book.DiscountStartDate,
                DiscountEndDate = book.DiscountEndDate
            };
        }
    }
}
