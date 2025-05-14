using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ClaimCode { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Processed, Cancelled
        public string ShippingAddress { get; set; }
        public string ContactPhone { get; set; }
        public string Notes { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public int? ProcessedByStaffId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        
        [NotMapped]
        public virtual User ProcessedByStaff { get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
