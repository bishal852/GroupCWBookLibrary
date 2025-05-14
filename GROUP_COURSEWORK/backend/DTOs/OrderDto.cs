using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string ClaimCode { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string ContactPhone { get; set; }
        public string Notes { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; }
        public bool CanCancel => Status == "Pending";
        
        // Customer information (for staff use)
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        
        // Add discount information
        public decimal DiscountAmount { get; set; }
        public bool HasBulkDiscount { get; set; }
        public bool HasLoyaltyDiscount { get; set; }
        public string DiscountDescription { get; set; }
    }
}
