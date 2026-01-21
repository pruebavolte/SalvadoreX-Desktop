using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupEventHandlers();
            UpdateSyncStatus();
        }
        
        private void SetupEventHandlers()
        {
            btnPOS.Click += BtnPOS_Click;
            btnInventory.Click += BtnInventory_Click;
            btnCustomers.Click += BtnCustomers_Click;
            btnSales.Click += BtnSales_Click;
            btnSettings.Click += BtnSettings_Click;
            
            SyncService.Instance.SyncStatusChanged += OnSyncStatusChanged;
            SyncService.Instance.ConnectionStatusChanged += OnConnectionStatusChanged;
        }
        
        private void BtnPOS_Click(object? sender, EventArgs e)
        {
            ShowPOS();
        }
        
        private void BtnInventory_Click(object? sender, EventArgs e)
        {
            ShowInventory();
        }
        
        private void BtnCustomers_Click(object? sender, EventArgs e)
        {
            ShowCustomers();
        }
        
        private void BtnSales_Click(object? sender, EventArgs e)
        {
            ShowSalesHistory();
        }
        
        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            ShowSettings();
        }
        
        private void ResetMenuButtons()
        {
            btnPOS.BackColor = Color.FromArgb(51, 65, 85);
            btnInventory.BackColor = Color.FromArgb(51, 65, 85);
            btnCustomers.BackColor = Color.FromArgb(51, 65, 85);
            btnSales.BackColor = Color.FromArgb(51, 65, 85);
            btnSettings.BackColor = Color.FromArgb(51, 65, 85);
        }
        
        private void ClearContent()
        {
            panelContent.Controls.Clear();
        }
        
        private void ShowPOS()
        {
            ResetMenuButtons();
            btnPOS.BackColor = Color.FromArgb(59, 130, 246);
            ClearContent();
            
            var posControl = new POSControl { Dock = DockStyle.Fill };
            panelContent.Controls.Add(posControl);
            
            lblStatus.Text = "  Punto de Venta";
        }
        
        private void ShowInventory()
        {
            ResetMenuButtons();
            btnInventory.BackColor = Color.FromArgb(59, 130, 246);
            ClearContent();
            
            var inventoryControl = new InventoryControl { Dock = DockStyle.Fill };
            panelContent.Controls.Add(inventoryControl);
            
            lblStatus.Text = "  Inventario";
        }
        
        private void ShowCustomers()
        {
            ResetMenuButtons();
            btnCustomers.BackColor = Color.FromArgb(59, 130, 246);
            ClearContent();
            
            var customersControl = new CustomersControl { Dock = DockStyle.Fill };
            panelContent.Controls.Add(customersControl);
            
            lblStatus.Text = "  Clientes";
        }
        
        private void ShowSalesHistory()
        {
            ResetMenuButtons();
            btnSales.BackColor = Color.FromArgb(59, 130, 246);
            ClearContent();
            
            var salesControl = new SalesHistoryControl { Dock = DockStyle.Fill };
            panelContent.Controls.Add(salesControl);
            
            lblStatus.Text = "  Historial de Ventas";
        }
        
        private void ShowSettings()
        {
            ResetMenuButtons();
            btnSettings.BackColor = Color.FromArgb(59, 130, 246);
            ClearContent();
            
            var settingsControl = new SettingsControl { Dock = DockStyle.Fill };
            panelContent.Controls.Add(settingsControl);
            
            lblStatus.Text = "  Configuracion";
        }
        
        private void UpdateSyncStatus()
        {
            var sync = SyncService.Instance;
            var statusIcon = sync.IsOnline ? "●" : "○";
            var statusColor = sync.IsOnline ? "En linea" : "Sin conexion";
            var pending = sync.PendingSyncCount > 0 ? $"\n{sync.PendingSyncCount} pendientes" : "";
            
            if (lblSync.InvokeRequired)
            {
                lblSync.Invoke(() => lblSync.Text = $"{statusIcon} {statusColor}{pending}");
                lblSync.Invoke(() => lblSync.ForeColor = sync.IsOnline ? Color.FromArgb(74, 222, 128) : Color.FromArgb(251, 146, 60));
            }
            else
            {
                lblSync.Text = $"{statusIcon} {statusColor}{pending}";
                lblSync.ForeColor = sync.IsOnline ? Color.FromArgb(74, 222, 128) : Color.FromArgb(251, 146, 60);
            }
        }
        
        private void OnSyncStatusChanged(object? sender, SyncEventArgs e)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(() => lblStatus.Text = $"  {e.Message}");
            }
            else
            {
                lblStatus.Text = $"  {e.Message}";
            }
        }
        
        private void OnConnectionStatusChanged(object? sender, bool isOnline)
        {
            UpdateSyncStatus();
        }
        
        private void SyncTimer_Tick(object? sender, EventArgs e)
        {
            UpdateSyncStatus();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowPOS();
        }
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            syncTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}
