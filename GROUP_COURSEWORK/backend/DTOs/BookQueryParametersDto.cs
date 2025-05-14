namespace backend.DTOs
{
    public class BookQueryParametersDto
    {
        // Search
        public string Search { get; set; }
        
        // Sorting
        public string SortBy { get; set; } = "CreatedAt"; // Default sort by creation date
        public bool SortDescending { get; set; } = true; // Default sort direction is descending
        
        // Filtering
        public string Genre { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string Format { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? OnSaleOnly { get; set; }
        public bool? InStockOnly { get; set; }
        
        // Pagination
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
