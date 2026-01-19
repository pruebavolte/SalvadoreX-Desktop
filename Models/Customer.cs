namespace SalvadoreXDesktop.Models
{
    public class Customer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Rfc { get; set; }
        public decimal CreditLimit { get; set; } = 0;
        public decimal CurrentCredit { get; set; } = 0;
        public int LoyaltyPoints { get; set; } = 0;
        public string? Notes { get; set; }
        public bool Active { get; set; } = true;
        public string RestaurantId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool NeedSync { get; set; } = false;
        public DateTime? LastSyncAt { get; set; }
    }
}
