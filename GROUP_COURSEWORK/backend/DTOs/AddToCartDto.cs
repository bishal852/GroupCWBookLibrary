using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class AddToCartDto
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
    }
}
