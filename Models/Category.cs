namespace SalvadoreXDesktop.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; } = "#3B82F6";
        public string? Icon { get; set; }
        public string? ParentId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool Active { get; set; } = true;
        public string RestaurantId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        public DateTime? LastSyncAt { get; set; }
    }
}
