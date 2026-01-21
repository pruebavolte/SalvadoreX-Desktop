using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }
        
        private void SettingsControl_Load(object sender, EventArgs e)
        {
            LoadSettings();
            UpdateSyncStatus();
        }
        
        private void LoadSettings()
        {
            txtBusinessName.Text = DatabaseManager.GetSetting("business_name", "Mi Negocio");
            txtBusinessAddress.Text = DatabaseManager.GetSetting("business_address");
            txtBusinessPhone.Text = DatabaseManager.GetSetting("business_phone");
            txtBusinessRfc.Text = DatabaseManager.GetSetting("business_rfc");
            
            if (decimal.TryParse(DatabaseManager.GetSetting("tax_rate", "16"), out var taxRate))
            {
                numTaxRate.Value = taxRate;
            }
            
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                try
                {
                    var json = File.ReadAllText(configPath);
                    var config = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);
                    txtSupabaseUrl.Text = config?.Supabase?.Url ?? "";
                    txtSupabaseKey.Text = config?.Supabase?.AnonKey ?? "";
                }
                catch { }
            }
        }
        
        private void UpdateSyncStatus()
        {
            var sync = SyncService.Instance;
            var statusIcon = sync.IsOnline ? "●" : "○";
            var statusText = sync.IsOnline ? "Conectado" : "Sin conexion";
            var pendingText = sync.PendingSyncCount > 0 ? $" ({sync.PendingSyncCount} pendientes)" : "";
            
            lblSyncStatus.Text = $"Estado: {statusIcon} {statusText}{pendingText}";
            lblSyncStatus.ForeColor = sync.IsOnline ? Color.FromArgb(34, 197, 94) : Color.FromArgb(239, 68, 68);
        }
        
        private async void BtnTestConnection_Click(object sender, EventArgs e)
        {
            btnTestConnection.Enabled = false;
            btnTestConnection.Text = "Probando...";
            
            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                
                var url = txtSupabaseUrl.Text.Trim();
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show("URL de Supabase no configurada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                var response = await client.GetAsync($"{url}/rest/v1/");
                
                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Conexion exitosa a Supabase", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Error de conexion: {response.StatusCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTestConnection.Enabled = true;
                btnTestConnection.Text = "Probar Conexion";
            }
        }
        
        private async void BtnSyncNow_Click(object sender, EventArgs e)
        {
            btnSyncNow.Enabled = false;
            btnSyncNow.Text = "Sincronizando...";
            
            try
            {
                await SyncService.Instance.ForceSyncNowAsync();
                UpdateSyncStatus();
                MessageBox.Show("Sincronizacion completada", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de sincronizacion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSyncNow.Enabled = true;
                btnSyncNow.Text = "Sincronizar Ahora";
            }
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DatabaseManager.SetSetting("business_name", txtBusinessName.Text.Trim());
                DatabaseManager.SetSetting("business_address", txtBusinessAddress.Text.Trim());
                DatabaseManager.SetSetting("business_phone", txtBusinessPhone.Text.Trim());
                DatabaseManager.SetSetting("business_rfc", txtBusinessRfc.Text.Trim());
                DatabaseManager.SetSetting("tax_rate", numTaxRate.Value.ToString());
                
                MessageBox.Show("Configuracion guardada exitosamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
