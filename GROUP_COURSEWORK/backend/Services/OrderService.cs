using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private const decimal TaxRate = 0.08m; // 8% tax rate
        private const decimal BulkOrderDiscount = 0.05m; // 5% discount for 5+ books
        private const decimal LoyaltyDiscount = 0.10m; // 10% discount after 10 successful orders
        private const int BulkOrderThreshold = 5; // Minimum books for bulk discount
        private const int LoyaltyOrderThreshold = 10; // Minimum successful orders for loyalty discount

        public OrderService(ApplicationDbContext context, ICartService cartService, IEmailService emailService, IUserService userService)
        {
            _context = context;
            _cartService = cartService;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task<IEnumerable<OrderSummaryDto>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    ClaimCode = o.ClaimCode,
                    OrderDate = o.OrderDate,
                    Total = o.FinalAmount,
                    Status = o.Status,
                    ItemCount = o.OrderItems.Sum(oi => oi.Quantity)
                })
                .ToListAsync();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

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
                }).ToList()
            };
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto dto)
        {
            // Get user's cart
            var cart = await _cartService.GetCartAsync(userId);
            if (cart.Items.Count() == 0)
            {
                throw new Exception("Cannot create order with empty cart");
            }

            // Get user information for email and discount calculation
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Generate claim code (UUID)
                var claimCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                // Calculate total quantity of books in the order
                int totalBookQuantity = cart.Items.Sum(item => item.Quantity);

                // Calculate discounts
                decimal subtotal = cart.Subtotal;
                decimal discountAmount = 0;
                decimal discountedSubtotal = subtotal;
                
                // Apply bulk order discount (5% for orders with 5+ books)
                if (totalBookQuantity >= BulkOrderThreshold)
                {
                    decimal bulkDiscount = subtotal * BulkOrderDiscount;
                    discountAmount += bulkDiscount;
                    discountedSubtotal -= bulkDiscount;
                    Console.WriteLine($"Applied bulk discount: ${bulkDiscount:F2}");
                }
                
                // Apply loyalty discount (10% after 10 successful orders)
                if (user.SuccessfulOrders >= LoyaltyOrderThreshold || user.HasStackableDiscount)
                {
                    decimal loyaltyDiscount = subtotal * LoyaltyDiscount;
                    discountAmount += loyaltyDiscount;
                    discountedSubtotal -= loyaltyDiscount;
                    Console.WriteLine($"Applied loyalty discount: ${loyaltyDiscount:F2}");
                }
                
                // Calculate tax on discounted subtotal
                decimal tax = discountedSubtotal * TaxRate;
                
                // Calculate final amount
                decimal finalAmount = discountedSubtotal + tax;

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    ClaimCode = claimCode,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = subtotal,
                    DiscountAmount = discountAmount,
                    FinalAmount = finalAmount,
                    Status = "Pending",
                    ShippingAddress = dto.ShippingAddress,
                    ContactPhone = dto.ContactPhone,
                    Notes = dto.Notes
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Create order items
                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.CurrentPrice,
                        Subtotal = cartItem.Subtotal
                    };

                    await _context.OrderItems.AddAsync(orderItem);

                    // Update book stock
                    var book = await _context.Books.FindAsync(cartItem.BookId);
                    if (book != null)
                    {
                        book.StockQuantity -= cartItem.Quantity;
                        _context.Books.Update(book);
                    }
                }

                await _context.SaveChangesAsync();

                // Clear the cart
                await _cartService.ClearCartAsync(userId);

                // Commit transaction
                await transaction.CommitAsync();

                // Send order confirmation email
                await _emailService.SendOrderConfirmationEmailAsync(
                    user.Email,
                    user.Name,
                    order.Id,
                    claimCode,
                    order.FinalAmount,
                    order.ShippingAddress
                );

                // Return order details
                return await GetOrderByIdAsync(userId, order.Id);
            }
            catch (Exception)
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CancelOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return false;
            }

            // Can only cancel pending orders
            if (order.Status != "Pending")
            {
                return false;
            }

            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Update order status
                order.Status = "Cancelled";
                order.CancelledDate = DateTime.UtcNow;
                _context.Orders.Update(order);

                // Restore book stock
                foreach (var orderItem in order.OrderItems)
                {
                    var book = await _context.Books.FindAsync(orderItem.BookId);
                    if (book != null)
                    {
                        book.StockQuantity += orderItem.Quantity;
                        _context.Books.Update(book);
                    }
                }

                await _context.SaveChangesAsync();

                // Commit transaction
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                // Rollback transaction on error
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<string> GenerateClaimCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Removed similar looking characters
            var random = new Random();
            var code = new StringBuilder();

            // Generate a 6-character code
            for (int i = 0; i < 6; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }

            var claimCode = code.ToString();

            // Check if code already exists
            while (await _context.Orders.AnyAsync(o => o.ClaimCode == claimCode))
            {
                // Regenerate if code already exists
                code.Clear();
                for (int i = 0; i < 6; i++)
                {
                    code.Append(chars[random.Next(chars.Length)]);
                }
                claimCode = code.ToString();
            }

            return claimCode;
        }
    }
}
