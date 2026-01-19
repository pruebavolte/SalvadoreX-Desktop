namespace SalvadoreXDesktop.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public int Stock { get; set; }
        public int MinStock { get; set; }
        public string? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; } = true;
        public bool AvailablePos { get; set; } = true;
        public bool AvailableDigitalMenu { get; set; } = true;
        public string RestaurantId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        public DateTime? LastSyncAt { get; set; }
        
        // Navigation property
        public Category? Category { get; set; }
    }
}
