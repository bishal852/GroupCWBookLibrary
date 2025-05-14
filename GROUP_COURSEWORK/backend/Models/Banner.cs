using System;

namespace backend.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BackgroundColor { get; set; } = "#f8d7da";
        public string TextColor { get; set; } = "#721c24";
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
