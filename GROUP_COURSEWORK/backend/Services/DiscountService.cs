using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;

namespace backend.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ApplicationDbContext _context;
        private const decimal BULK_DISCOUNT_RATE = 0.05m; // 5% discount for 5-9 books
        private const int BULK_DISCOUNT_THRESHOLD = 5; // Minimum books for bulk discount
        private const decimal LOYALTY_DISCOUNT_RATE = 0.10m; // 10% discount for 10+ books
        private const int LOYALTY_DISCOUNT_THRESHOLD = 10; // Minimum books for loyalty discount

        public DiscountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountDto> CalculateDiscountsAsync(int userId, decimal subtotal, int totalItems)
        {
            var discountDto = new DiscountDto
            {
                DiscountAmount = 0,
                DiscountedSubtotal = subtotal,
                HasBulkDiscount = false,
                HasLoyaltyDiscount = false,
                BulkDiscountAmount = 0,
                LoyaltyDiscountAmount = 0,
                DiscountDescription = ""
            };

            // Apply loyalty discount (10%) for orders with 10+ books
            if (totalItems >= LOYALTY_DISCOUNT_THRESHOLD)
            {
                decimal loyaltyDiscount = Math.Round(subtotal * LOYALTY_DISCOUNT_RATE, 2);
                discountDto.LoyaltyDiscountAmount = loyaltyDiscount;
                discountDto.DiscountAmount = loyaltyDiscount; // Only apply the loyalty discount
                discountDto.HasLoyaltyDiscount = true;
                discountDto.HasBulkDiscount = false; // Ensure bulk discount is not applied
                discountDto.DiscountDescription = $"10% discount for ordering {totalItems} books: -${loyaltyDiscount:F2}\n";
                Console.WriteLine($"Applied 10% discount for {totalItems} books: ${loyaltyDiscount:F2}");
            }
            // Apply bulk discount (5%) only for orders with 5-9 books
            else if (totalItems >= BULK_DISCOUNT_THRESHOLD)
            {
                decimal bulkDiscount = Math.Round(subtotal * BULK_DISCOUNT_RATE, 2);
                discountDto.BulkDiscountAmount = bulkDiscount;
                discountDto.DiscountAmount = bulkDiscount;
                discountDto.HasBulkDiscount = true;
                discountDto.HasLoyaltyDiscount = false;
                discountDto.DiscountDescription = $"5% discount for ordering {totalItems} books: -${bulkDiscount:F2}\n";
                Console.WriteLine($"Applied 5% discount for {totalItems} books: ${bulkDiscount:F2}");
            }

            // Calculate final discounted subtotal
            discountDto.DiscountedSubtotal = subtotal - discountDto.DiscountAmount;

            return discountDto;
        }
    }
}
