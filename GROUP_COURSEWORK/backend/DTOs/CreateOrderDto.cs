using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public string ShippingAddress { get; set; }
        
        [Required]
        public string ContactPhone { get; set; }
        
        public string Notes { get; set; }
    }
}
