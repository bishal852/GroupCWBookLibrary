using System.Collections.Generic;

namespace backend.DTOs
{
    public class CartSummaryDto
    {
        public IEnumerable<CartItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedSubtotal { get; set; }
        public bool HasBulkDiscount { get; set; }
        public bool HasLoyaltyDiscount { get; set; }
        public decimal BulkDiscountAmount { get; set; }
        public decimal LoyaltyDiscountAmount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
