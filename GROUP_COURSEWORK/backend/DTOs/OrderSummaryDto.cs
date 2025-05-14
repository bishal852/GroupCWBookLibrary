using System;

namespace backend.DTOs
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public string ClaimCode { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public int ItemCount { get; set; }
        
        // Add discount information
        public decimal DiscountAmount { get; set; }
        public bool HasBulkDiscount { get; set; }
        public bool HasLoyaltyDiscount { get; set; }
    }
}
