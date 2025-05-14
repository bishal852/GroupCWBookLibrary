using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Author { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength = 10)]
        public string ISBN { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        
        [Required]
        public string Genre { get; set; }
        
        [Required]
        public string Language { get; set; }
        
        [Required]
        public string Format { get; set; }
        
        [Required]
        public string Publisher { get; set; }
        
        [Required]
        public DateTime PublicationDate { get; set; }
        
        [Required]
        [Range(0, 10000)]
        public int StockQuantity { get; set; }
        
        public string CoverImageUrl { get; set; }
        
        public bool IsOnSale { get; set; }
        
        public decimal? DiscountPrice { get; set; }
        
        public DateTime? DiscountStartDate { get; set; }
        
        public DateTime? DiscountEndDate { get; set; }
    }
}
