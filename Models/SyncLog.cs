namespace SalvadoreXDesktop.Models
{
    public class SyncLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TableName { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string? Data { get; set; }
        public int Synced { get; set; } = 0;
        public string? SyncedAt { get; set; }
        public int RetryCount { get; set; } = 0;
        public string? Error { get; set; }
        public string CreatedAt { get; set; } = DateTime.UtcNow.ToString("o");
    }
}
