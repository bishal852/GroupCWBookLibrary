using System;

namespace backend.DTOs
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsOnSale { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public DateTime AddedAt { get; set; }
        
        // Helper property to determine if a discount is currently active
        public bool IsDiscountActive => 
            IsOnSale && 
            DiscountPrice.HasValue && 
            DiscountStartDate.HasValue && 
            DiscountEndDate.HasValue && 
            DateTime.UtcNow >= DiscountStartDate.Value && 
            DateTime.UtcNow <= DiscountEndDate.Value;
            
        // Helper property to get the current price (either regular or discount)
        public decimal CurrentPrice => IsDiscountActive ? DiscountPrice.Value : Price;
    }
}
