using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UpdateStockDto
    {
        [Required]
        [Range(0, 10000)]
        public int StockQuantity { get; set; }
    }
}
