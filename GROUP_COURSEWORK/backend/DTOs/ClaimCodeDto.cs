using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class ClaimCodeDto
    {
        [Required]
        public string ClaimCode { get; set; }
    }
}
