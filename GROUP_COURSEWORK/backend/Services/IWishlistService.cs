using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistItemDto>> GetWishlistItemsAsync(int userId);
        Task<bool> IsBookInWishlistAsync(int userId, int bookId);
        Task<Wishlist> AddToWishlistAsync(Wishlist wishlistItem);
        Task RemoveFromWishlistAsync(int userId, int bookId);
    }
}
