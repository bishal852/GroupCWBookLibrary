using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UpdateBannerDto
    {
        [Required]
        public string Message { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public string BackgroundColor { get; set; }
        
        public string TextColor { get; set; }
    }
}
