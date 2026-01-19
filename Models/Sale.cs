namespace SalvadoreXDesktop.Models
{
    public class Sale
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string PaymentMethod { get; set; } = "cash";
        public string Status { get; set; } = "completed";
        public string? Notes { get; set; }
        public string? ReceiptNumber { get; set; }
        public string RestaurantId { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        public DateTime? LastSyncAt { get; set; }
        
        // Navigation properties
        public Customer? Customer { get; set; }
        public List<SaleItem> Items { get; set; } = new();
    }
    
    public class SaleItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SaleId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        
        // Navigation property
        public Product? Product { get; set; }
    }
}
