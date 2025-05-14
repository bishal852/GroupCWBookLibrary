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
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDto>> GetBookReviewsAsync(int bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.User.Name,
                    BookId = r.BookId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                    IsOwner = false // This will be set by the controller
                })
                .ToListAsync();
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int reviewId)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (review == null)
            {
                return null;
            }

            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User.Name,
                BookId = review.BookId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                IsOwner = false // This will be set by the controller
            };
        }

        public async Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto)
        {
            // Check if the user has purchased the book
            if (!await HasUserPurchasedBookAsync(userId, dto.BookId))
            {
                throw new Exception("You can only review books you have purchased");
            }

            // Check if the user has already reviewed this book
            if (await HasUserReviewedBookAsync(userId, dto.BookId))
            {
                throw new Exception("You have already reviewed this book");
            }

            var review = new Review
            {
                UserId = userId,
                BookId = dto.BookId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            // Update the book's average rating
            await UpdateBookAverageRatingAsync(dto.BookId);

            // Get the user's name
            var user = await _context.Users.FindAsync(userId);

            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = user.Name,
                BookId = review.BookId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                IsOwner = true
            };
        }

        public async Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, CreateReviewDto dto)
        {
            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (review == null)
            {
                throw new Exception("Review not found or you don't have permission to update it");
            }

            // Update review properties
            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            // Update the book's average rating
            await UpdateBookAverageRatingAsync(review.BookId);

            return new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                UserName = review.User.Name,
                BookId = review.BookId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                IsOwner = true
            };
        }

        public async Task<bool> DeleteReviewAsync(int userId, int reviewId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);

            if (review == null)
            {
                return false;
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            // Update the book's average rating
            await UpdateBookAverageRatingAsync(review.BookId);

            return true;
        }

        public async Task<bool> HasUserPurchasedBookAsync(int userId, int bookId)
        {
            // Check if the user has any completed orders containing this book
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Status == "Processed")
                .Join(_context.OrderItems,
                    order => order.Id,
                    orderItem => orderItem.OrderId,
                    (order, orderItem) => new { Order = order, OrderItem = orderItem })
                .AnyAsync(o => o.OrderItem.BookId == bookId);
        }

        public async Task<bool> HasUserReviewedBookAsync(int userId, int bookId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<double> CalculateAverageRatingAsync(int bookId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.BookId == bookId)
                .Select(r => r.Rating)
                .ToListAsync();

            if (ratings.Count == 0)
            {
                return 0;
            }

            return Math.Round(ratings.Average(), 1);
        }

        private async Task UpdateBookAverageRatingAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.AverageRating = await CalculateAverageRatingAsync(bookId);
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
