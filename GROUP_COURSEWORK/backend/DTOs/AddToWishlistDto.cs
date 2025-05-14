using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class AddToWishlistDto
    {
        [Required]
        public int BookId { get; set; }
    }
}
