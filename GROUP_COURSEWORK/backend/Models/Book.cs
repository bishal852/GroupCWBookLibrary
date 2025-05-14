using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public string Format { get; set; } // Paperback, Hardcover, etc.
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public int StockQuantity { get; set; }
        public string CoverImageUrl { get; set; }
        public double AverageRating { get; set; }
        public bool IsOnSale { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
