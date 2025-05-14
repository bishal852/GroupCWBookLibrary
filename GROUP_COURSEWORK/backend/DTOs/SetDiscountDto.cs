using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class SetDiscountDto
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [Range(1, 99)]
        public int DiscountPercent { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public bool OnSale { get; set; }
    }
}
