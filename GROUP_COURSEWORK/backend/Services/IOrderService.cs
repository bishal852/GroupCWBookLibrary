using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderSummaryDto>> GetUserOrdersAsync(int userId);
        Task<OrderDto> GetOrderByIdAsync(int userId, int orderId);
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto dto);
        Task<bool> CancelOrderAsync(int userId, int orderId);
        Task<string> GenerateClaimCode();
    }
}
