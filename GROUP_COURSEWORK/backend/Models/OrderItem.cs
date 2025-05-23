namespace backend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        
        // Navigation properties
        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}
