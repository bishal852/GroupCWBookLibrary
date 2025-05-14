using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateBannerDto
    {
        [Required]
        public string Message { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public string BackgroundColor { get; set; } = "#f8d7da";
        
        public string TextColor { get; set; } = "#721c24";
    }
}
