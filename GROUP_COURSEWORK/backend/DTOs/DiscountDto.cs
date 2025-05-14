namespace backend.DTOs
{
    public class DiscountDto
    {
        public decimal DiscountAmount { get; set; }
        public decimal DiscountedSubtotal { get; set; }
        public bool HasBulkDiscount { get; set; }
        public bool HasLoyaltyDiscount { get; set; }
        public decimal BulkDiscountAmount { get; set; }
        public decimal LoyaltyDiscountAmount { get; set; }
        public string DiscountDescription { get; set; }
    }
}
