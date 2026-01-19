using System.Net.NetworkInformation;
using Dapper;
using Newtonsoft.Json;
using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Services
{
    public class SyncService
    {
        private static SyncService? _instance;
        public static SyncService Instance => _instance ??= new SyncService();
        
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _backgroundTask;
        private readonly int _syncIntervalSeconds = 30;
        
        public event EventHandler<SyncEventArgs>? SyncStatusChanged;
        public event EventHandler<bool>? ConnectionStatusChanged;
        
        public bool IsOnline { get; private set; } = false;
        public bool IsSyncing { get; private set; } = false;
        public DateTime? LastSyncTime { get; private set; }
        public int PendingSyncCount { get; private set; } = 0;
        
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        
        public SyncService()
        {
            // Cargar configuracion
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                var config = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(configPath));
                _supabaseUrl = config?.Supabase?.Url ?? "";
                _supabaseKey = config?.Supabase?.AnonKey ?? "";
            }
            else
            {
                _supabaseUrl = "";
                _supabaseKey = "";
            }
        }
        
        public void StartBackgroundSync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _backgroundTask = Task.Run(() => BackgroundSyncLoop(_cancellationTokenSource.Token));
        }
        
        public void StopBackgroundSync()
        {
            _cancellationTokenSource?.Cancel();
            _backgroundTask?.Wait(TimeSpan.FromSeconds(5));
        }
        
        private async Task BackgroundSyncLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Verificar conexion
                    var wasOnline = IsOnline;
                    IsOnline = await CheckInternetConnectionAsync();
                    
                    if (wasOnline != IsOnline)
                    {
                        ConnectionStatusChanged?.Invoke(this, IsOnline);
                    }
                    
                    // Actualizar conteo de pendientes
                    UpdatePendingCount();
                    
                    // Si hay conexion y hay cambios pendientes, sincronizar
                    if (IsOnline && PendingSyncCount > 0)
                    {
                        await SyncPendingChangesAsync();
                    }
                    
                    // Si hay conexion, descargar cambios del servidor
                    if (IsOnline)
                    {
                        await PullChangesFromServerAsync();
                    }
                }
                catch (Exception ex)
                {
                    OnSyncStatusChanged("error", ex.Message);
                }
                
                await Task.Delay(TimeSpan.FromSeconds(_syncIntervalSeconds), cancellationToken);
            }
        }
        
        private async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var response = await client.GetAsync("https://www.google.com/generate_204");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        private void UpdatePendingCount()
        {
            using var connection = DatabaseManager.GetConnection();
            PendingSyncCount = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sync_log WHERE synced = 0"
            );
        }
        
        public async Task SyncPendingChangesAsync()
        {
            if (IsSyncing || !IsOnline) return;
            
            IsSyncing = true;
            OnSyncStatusChanged("syncing", "Sincronizando cambios...");
            
            try
            {
                using var connection = DatabaseManager.GetConnection();
                var pendingLogs = connection.Query<SyncLog>(
                    "SELECT * FROM sync_log WHERE synced = 0 ORDER BY created_at LIMIT 100"
                ).ToList();
                
                foreach (var log in pendingLogs)
                {
                    try
                    {
                        await SyncRecordToServerAsync(log);
                        
                        // Marcar como sincronizado
                        connection.Execute(
                            "UPDATE sync_log SET synced = 1, synced_at = @SyncedAt WHERE id = @Id",
                            new { Id = log.Id, SyncedAt = DateTime.UtcNow.ToString("o") }
                        );
                    }
                    catch (Exception ex)
                    {
                        // Incrementar contador de reintentos
                        connection.Execute(
                            "UPDATE sync_log SET retry_count = retry_count + 1, error = @Error WHERE id = @Id",
                            new { Id = log.Id, Error = ex.Message }
                        );
                    }
                }
                
                LastSyncTime = DateTime.Now;
                UpdatePendingCount();
                OnSyncStatusChanged("success", $"Sincronizado: {pendingLogs.Count} registros");
            }
            catch (Exception ex)
            {
                OnSyncStatusChanged("error", ex.Message);
            }
            finally
            {
                IsSyncing = false;
            }
        }
        
        private async Task SyncRecordToServerAsync(SyncLog log)
        {
            if (string.IsNullOrEmpty(_supabaseUrl) || string.IsNullOrEmpty(_supabaseKey))
                return;
                
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", _supabaseKey);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
            client.DefaultRequestHeaders.Add("Prefer", "return=representation");
            
            // Obtener el registro actual de la base de datos local
            using var connection = DatabaseManager.GetConnection();
            object? record = log.TableName switch
            {
                "products" => connection.QueryFirstOrDefault($"SELECT * FROM products WHERE id = @Id", new { Id = log.RecordId }),
                "categories" => connection.QueryFirstOrDefault($"SELECT * FROM categories WHERE id = @Id", new { Id = log.RecordId }),
                "customers" => connection.QueryFirstOrDefault($"SELECT * FROM customers WHERE id = @Id", new { Id = log.RecordId }),
                "sales" => connection.QueryFirstOrDefault($"SELECT * FROM sales WHERE id = @Id", new { Id = log.RecordId }),
                "sale_items" => connection.QueryFirstOrDefault($"SELECT * FROM sale_items WHERE id = @Id", new { Id = log.RecordId }),
                _ => null
            };
            
            if (record == null) return;
            
            var json = JsonConvert.SerializeObject(record);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var url = $"{_supabaseUrl}/rest/v1/{log.TableName}";
            
            HttpResponseMessage response;
            
            if (log.Operation == "insert")
            {
                response = await client.PostAsync(url, content);
            }
            else if (log.Operation == "update" || log.Operation == "delete")
            {
                url += $"?id=eq.{log.RecordId}";
                response = await client.PatchAsync(url, content);
            }
            else
            {
                return;
            }
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error de sincronizacion: {response.StatusCode} - {errorContent}");
            }
        }
        
        public async Task PullChangesFromServerAsync()
        {
            if (string.IsNullOrEmpty(_supabaseUrl) || string.IsNullOrEmpty(_supabaseKey))
                return;
                
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("apikey", _supabaseKey);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseKey}");
                
                var restaurantId = DatabaseManager.GetSetting("restaurant_id");
                var lastSync = DatabaseManager.GetSetting("last_server_sync", "1970-01-01T00:00:00Z");
                
                // Sincronizar categorias
                await PullTableAsync(client, "categories", restaurantId, lastSync);
                
                // Sincronizar productos
                await PullTableAsync(client, "products", restaurantId, lastSync);
                
                // Sincronizar clientes
                await PullTableAsync(client, "customers", restaurantId, lastSync);
                
                // Actualizar ultima sincronizacion
                DatabaseManager.SetSetting("last_server_sync", DateTime.UtcNow.ToString("o"));
            }
            catch (Exception ex)
            {
                // Ignorar errores de pull, ya que podemos trabajar offline
                System.Diagnostics.Debug.WriteLine($"Error pulling from server: {ex.Message}");
            }
        }
        
        private async Task PullTableAsync(HttpClient client, string tableName, string restaurantId, string lastSync)
        {
            var url = $"{_supabaseUrl}/rest/v1/{tableName}?restaurant_id=eq.{restaurantId}&updated_at=gt.{lastSync}";
            
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return;
            
            var json = await response.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            
            if (records == null || records.Count == 0) return;
            
            using var connection = DatabaseManager.GetConnection();
            
            foreach (var record in records)
            {
                // Verificar si el registro local tiene cambios sin sincronizar
                var localNeedSync = connection.ExecuteScalar<int>(
                    $"SELECT need_sync FROM {tableName} WHERE id = @Id",
                    new { Id = record["id"]?.ToString() }
                );
                
                // Si el registro local necesita sincronizacion, no sobreescribir
                if (localNeedSync == 1) continue;
                
                // Upsert del registro
                // (En una implementacion real, se haria un UPSERT adecuado para cada tabla)
            }
        }
        
        public async Task ForceSyncNowAsync()
        {
            if (!IsOnline)
            {
                OnSyncStatusChanged("error", "Sin conexion a internet");
                return;
            }
            
            await SyncPendingChangesAsync();
            await PullChangesFromServerAsync();
        }
        
        private void OnSyncStatusChanged(string status, string message)
        {
            SyncStatusChanged?.Invoke(this, new SyncEventArgs { Status = status, Message = message });
        }
    }
    
    public class SyncEventArgs : EventArgs
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
