using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.DTOs;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Member")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var wishlistItems = await _wishlistService.GetWishlistItemsAsync(userId);
            return Ok(wishlistItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            // Check if the book is already in the wishlist
            if (await _wishlistService.IsBookInWishlistAsync(userId, dto.BookId))
            {
                return BadRequest(new { message = "Book is already in your wishlist" });
            }
            
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                BookId = dto.BookId
            };
            
            await _wishlistService.AddToWishlistAsync(wishlistItem);
            return Ok(new { message = "Book added to wishlist" });
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            // Check if the book is in the wishlist
            if (!await _wishlistService.IsBookInWishlistAsync(userId, bookId))
            {
                return NotFound(new { message = "Book not found in your wishlist" });
            }
            
            await _wishlistService.RemoveFromWishlistAsync(userId, bookId);
            return Ok(new { message = "Book removed from wishlist" });
        }
    }
}
