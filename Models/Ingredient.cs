namespace SalvadoreXDesktop.Models
{
    public class Ingredient
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Sku { get; set; }
        public string? Category { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
        public decimal MaxStock { get; set; }
        public string UnitType { get; set; } = "weight";
        public string UnitName { get; set; } = "kg";
        public decimal CostPerUnit { get; set; }
        public bool Active { get; set; } = true;
        public string RestaurantId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        public DateTime? LastSyncAt { get; set; }
    }
}
