using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Services
{
    public interface IStaffService
    {
        Task<OrderDto> GetOrderByClaimCodeAsync(string claimCode);
        Task<OrderDto> FulfillOrderByClaimCodeAsync(int staffId, string claimCode);
    }
}
