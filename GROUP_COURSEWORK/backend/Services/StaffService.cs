using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class StaffService : IStaffService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public StaffService(ApplicationDbContext context, INotificationService notificationService, IEmailService emailService)
        {
            _context = context;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        public async Task<OrderDto> GetOrderByClaimCodeAsync(string claimCode)
        {
            if (string.IsNullOrWhiteSpace(claimCode))
            {
                throw new ArgumentException("Claim code cannot be empty", nameof(claimCode));
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.ClaimCode == claimCode.Trim().ToUpper());

            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                ClaimCode = order.ClaimCode,
                OrderDate = order.OrderDate,
                Subtotal = order.TotalAmount,
                Tax = order.DiscountAmount, // Using DiscountAmount field to store tax
                Total = order.FinalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                ContactPhone = order.ContactPhone,
                Notes = order.Notes,
                ProcessedDate = order.CompletedDate,
                CancelledDate = order.CancelledDate,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    Title = oi.Book.Title,
                    Author = oi.Book.Author,
                    CoverImageUrl = oi.Book.CoverImageUrl,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).ToList(),
                CustomerName = order.User.Name,
                CustomerEmail = order.User.Email
            };
        }

        public async Task<OrderDto> FulfillOrderByClaimCodeAsync(int staffId, string claimCode)
        {
            if (string.IsNullOrWhiteSpace(claimCode))
            {
                throw new ArgumentException("Claim code cannot be empty", nameof(claimCode));
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.ClaimCode == claimCode.Trim().ToUpper());

            if (order == null)
            {
                throw new Exception("Order not found with the provided claim code");
            }

            if (order.Status != "Pending")
            {
                throw new Exception($"Order cannot be fulfilled. Current status: {order.Status}");
            }

            // Update order status
            order.Status = "Processed";
            order.CompletedDate = DateTime.UtcNow;
            order.ProcessedByStaffId = staffId;

            // Update user's successful orders count
            var user = order.User;
            user.SuccessfulOrders += 1;

            // Check if user is eligible for stackable discount
            if (user.SuccessfulOrders >= 10 && !user.HasStackableDiscount)
            {
                user.HasStackableDiscount = true;
            }

            _context.Orders.Update(order);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Send email notification to customer
            try
            {
                await _emailService.SendOrderProcessedEmailAsync(
                    order.User.Email,
                    order.User.Name,
                    order.Id,
                    order.FinalAmount,
                    order.ShippingAddress,
                    order.CompletedDate.Value
                );
                Console.WriteLine($"Order processed email sent to {order.User.Email}");
            }
            catch (Exception ex)
            {
                // Log the error but don't fail the order processing
                Console.WriteLine($"Error sending order processed email: {ex.Message}");
            }

            // Send notifications for each book in the order
            foreach (var orderItem in order.OrderItems)
            {
                await _notificationService.SendOrderFulfilledNotification(
                    order.Id, 
                    orderItem.Book.Title, 
                    orderItem.Quantity
                );
            }

            return new OrderDto
            {
                Id = order.Id,
                ClaimCode = order.ClaimCode,
                OrderDate = order.OrderDate,
                Subtotal = order.TotalAmount,
                Tax = order.DiscountAmount, // Using DiscountAmount field to store tax
                Total = order.FinalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                ContactPhone = order.ContactPhone,
                Notes = order.Notes,
                ProcessedDate = order.CompletedDate,
                CancelledDate = order.CancelledDate,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    Title = oi.Book.Title,
                    Author = oi.Book.Author,
                    CoverImageUrl = oi.Book.CoverImageUrl,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity,
                    Subtotal = oi.Subtotal
                }).ToList(),
                CustomerName = order.User.Name,
                CustomerEmail = order.User.Email
            };
        }
    }
}
