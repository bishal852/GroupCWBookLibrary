using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface ICartService
    {
        Task<CartSummaryDto> GetCartAsync(int userId);
        Task<CartItemDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<CartItemDto> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto dto);
        Task RemoveFromCartAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }
}
