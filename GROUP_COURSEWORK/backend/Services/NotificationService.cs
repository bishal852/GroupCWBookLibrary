using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;

namespace backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<OrderHub> _hubContext;

        public NotificationService(IHubContext<OrderHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendOrderFulfilledNotification(int orderId, string bookTitle, int quantity)
        {
            string quantityText = quantity == 1 ? "copy" : "copies";
            string message = $"New Order: {quantity} {quantityText} of \"{bookTitle}\" just sold!";
            
            await _hubContext.Clients.All.SendAsync("ReceiveOrderNotification", message);
        }
    }
}
