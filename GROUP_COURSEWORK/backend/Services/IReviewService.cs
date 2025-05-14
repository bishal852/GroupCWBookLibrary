using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetBookReviewsAsync(int bookId);
        Task<ReviewDto> GetReviewByIdAsync(int reviewId);
        Task<ReviewDto> CreateReviewAsync(int userId, CreateReviewDto dto);
        Task<ReviewDto> UpdateReviewAsync(int userId, int reviewId, CreateReviewDto dto);
        Task<bool> DeleteReviewAsync(int userId, int reviewId);
        Task<bool> HasUserPurchasedBookAsync(int userId, int bookId);
        Task<bool> HasUserReviewedBookAsync(int userId, int bookId);
        Task<double> CalculateAverageRatingAsync(int bookId);
    }
}
