using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Services
{
    public interface IDiscountService
    {
        Task<DiscountDto> CalculateDiscountsAsync(int userId, decimal subtotal, int totalItems);
    }
}
