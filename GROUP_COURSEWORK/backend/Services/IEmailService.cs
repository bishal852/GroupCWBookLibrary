using System;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationEmailAsync(string email, string name, int orderId, string claimCode, decimal total, string shippingAddress);
        Task SendOrderProcessedEmailAsync(string email, string name, int orderId, decimal total, string shippingAddress, DateTime processedDate);
    }
}
