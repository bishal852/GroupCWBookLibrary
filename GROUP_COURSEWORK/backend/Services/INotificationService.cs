using System.Threading.Tasks;

namespace backend.Services
{
    public interface INotificationService
    {
        Task SendOrderFulfilledNotification(int orderId, string bookTitle, int quantity);
    }
}
