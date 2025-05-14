using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateReviewDto
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Comment { get; set; }
    }
}
