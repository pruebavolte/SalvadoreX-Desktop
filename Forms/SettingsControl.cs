using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public class SettingsControl : UserControl
    {
        private TextBox txtBusinessName = null!;
        private TextBox txtBusinessAddress = null!;
        private TextBox txtBusinessPhone = null!;
        private TextBox txtBusinessRfc = null!;
        private NumericUpDown numTaxRate = null!;
        private TextBox txtSupabaseUrl = null!;
        private TextBox txtSupabaseKey = null!;
        private Label lblSyncStatus = null!;
        private Button btnSave = null!;
        private Button btnSyncNow = null!;
        private Button btnTestConnection = null!;
        
        public SettingsControl()
        {
            InitializeComponent();
            LoadSettings();
        }
        
        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Padding = new Padding(20);
            this.AutoScroll = true;
            
            var lblTitle = new Label
            {
                Text = "Configuracion",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(20, 10),
                AutoSize = true
            };
            
            // Seccion: Datos del Negocio
            var lblBusinessSection = new Label
            {
                Text = "Datos del Negocio",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(20, 60),
                AutoSize = true
            };
            
            var y = 100;
            var labelX = 20;
            var inputX = 200;
            var inputWidth = 350;
            var rowHeight = 45;
            
            AddLabel("Nombre del Negocio:", labelX, y);
            txtBusinessName = AddTextBox(inputX, y - 5, inputWidth);
            y += rowHeight;
            
            AddLabel("Direccion:", labelX, y);
            txtBusinessAddress = AddTextBox(inputX, y - 5, inputWidth);
            y += rowHeight;
            
            AddLabel("Telefono:", labelX, y);
            txtBusinessPhone = AddTextBox(inputX, y - 5, inputWidth);
            y += rowHeight;
            
            AddLabel("RFC:", labelX, y);
            txtBusinessRfc = AddTextBox(inputX, y - 5, inputWidth);
            y += rowHeight;
            
            AddLabel("Tasa de IVA (%):", labelX, y);
            numTaxRate = new NumericUpDown
            {
                Location = new Point(inputX, y - 5),
                Size = new Size(100, 30),
                Value = 16,
                DecimalPlaces = 2,
                Maximum = 100
            };
            this.Controls.Add(numTaxRate);
            y += rowHeight + 20;
            
            // Seccion: Sincronizacion
            var lblSyncSection = new Label
            {
                Text = "Sincronizacion en la Nube",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246),
                Location = new Point(20, y),
                AutoSize = true
            };
            y += 40;
            
            lblSyncStatus = new Label
            {
                Text = "Estado: Verificando...",
                Font = new Font("Segoe UI", 11F),
                Location = new Point(labelX, y),
                AutoSize = true
            };
            y += 35;
            
            AddLabel("URL Supabase:", labelX, y);
            txtSupabaseUrl = AddTextBox(inputX, y - 5, inputWidth);
            txtSupabaseUrl.ReadOnly = true;
            y += rowHeight;
            
            AddLabel("API Key:", labelX, y);
            txtSupabaseKey = AddTextBox(inputX, y - 5, inputWidth);
            txtSupabaseKey.PasswordChar = '*';
            txtSupabaseKey.ReadOnly = true;
            y += rowHeight + 10;
            
            btnTestConnection = new Button
            {
                Text = "Probar Conexion",
                Location = new Point(inputX, y),
                Size = new Size(140, 40),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnTestConnection.FlatAppearance.BorderSize = 0;
            btnTestConnection.Click += BtnTestConnection_Click;
            
            btnSyncNow = new Button
            {
                Text = "Sincronizar Ahora",
                Location = new Point(inputX + 150, y),
                Size = new Size(140, 40),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSyncNow.FlatAppearance.BorderSize = 0;
            btnSyncNow.Click += BtnSyncNow_Click;
            
            y += rowHeight + 40;
            
            // Botones principales
            btnSave = new Button
            {
                Text = "Guardar Configuracion",
                Location = new Point(inputX, y),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            
            // Agregar controles
            this.Controls.AddRange(new Control[] 
            { 
                lblTitle, lblBusinessSection, lblSyncSection, lblSyncStatus,
                btnTestConnection, btnSyncNow, btnSave
            });
            
            UpdateSyncStatus();
        }
        
        private Label AddLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(label);
            return label;
        }
        
        private TextBox AddTextBox(int x, int y, int width)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 30),
                Font = new Font("Segoe UI", 10F)
            };
            this.Controls.Add(textBox);
            return textBox;
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
            
            // Cargar configuracion de Supabase
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
        
        private async void BtnTestConnection_Click(object? sender, EventArgs e)
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
        
        private async void BtnSyncNow_Click(object? sender, EventArgs e)
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
        
        private void BtnSave_Click(object? sender, EventArgs e)
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
