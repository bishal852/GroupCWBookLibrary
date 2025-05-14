using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetBookReviews(int bookId)
        {
            var reviews = await _reviewService.GetBookReviewsAsync(bookId);
            
            // If user is authenticated, mark their own reviews
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                reviews = reviews.Select(r => 
                {
                    r.IsOwner = r.UserId == userId;
                    return r;
                });
            }
            
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            
            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }
            
            // If user is authenticated, check if they own the review
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                review.IsOwner = review.UserId == userId;
            }
            
            return Ok(review);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var review = await _reviewService.CreateReviewAsync(userId, dto);
                return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var review = await _reviewService.UpdateReviewAsync(userId, id, dto);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _reviewService.DeleteReviewAsync(userId, id);
                
                if (!result)
                {
                    return NotFound(new { message = "Review not found or you don't have permission to delete it" });
                }
                
                return Ok(new { message = "Review deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("can-review/{bookId}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> CanReviewBook(int bookId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var hasPurchased = await _reviewService.HasUserPurchasedBookAsync(userId, bookId);
            var hasReviewed = await _reviewService.HasUserReviewedBookAsync(userId, bookId);
            
            return Ok(new { 
                canReview = hasPurchased && !hasReviewed,
                hasPurchased = hasPurchased,
                hasReviewed = hasReviewed
            });
        }
    }
}
