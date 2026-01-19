namespace SalvadoreXDesktop.Models
{
    public class SyncStatus
    {
        public string TableName { get; set; } = string.Empty;
        public DateTime LastSyncAt { get; set; }
        public int PendingCount { get; set; }
        public string Status { get; set; } = "idle"; // idle, syncing, error
        public string? LastError { get; set; }
    }
    
    public class SyncLog
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty; // insert, update, delete
        public string? Data { get; set; }
        public bool Synced { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SyncedAt { get; set; }
        public string? Error { get; set; }
        public int RetryCount { get; set; } = 0;
    }
}
