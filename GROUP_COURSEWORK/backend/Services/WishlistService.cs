using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(int userId)
        {
            return await _context.Wishlists
                .Where(w => w.UserId == userId)
                .Include(w => w.Book)
                .OrderByDescending(w => w.CreatedAt)
                .Select(w => new WishlistItemDto
                {
                    Id = w.Id,
                    BookId = w.BookId,
                    Title = w.Book.Title,
                    Author = w.Book.Author,
                    CoverImageUrl = w.Book.CoverImageUrl,
                    Price = w.Book.Price,
                    IsOnSale = w.Book.IsOnSale,
                    DiscountPrice = w.Book.DiscountPrice,
                    DiscountStartDate = w.Book.DiscountStartDate,
                    DiscountEndDate = w.Book.DiscountEndDate,
                    AddedAt = w.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> IsBookInWishlistAsync(int userId, int bookId)
        {
            return await _context.Wishlists
                .AnyAsync(w => w.UserId == userId && w.BookId == bookId);
        }

        public async Task<Wishlist> AddToWishlistAsync(Wishlist wishlistItem)
        {
            await _context.Wishlists.AddAsync(wishlistItem);
            await _context.SaveChangesAsync();
            return wishlistItem;
        }

        public async Task RemoveFromWishlistAsync(int userId, int bookId)
        {
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
                
            if (wishlistItem != null)
            {
                _context.Wishlists.Remove(wishlistItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
