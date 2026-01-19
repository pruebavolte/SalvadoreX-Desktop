using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public partial class MainForm : Form
    {
        private Button btnPOS = null!;
        private Button btnInventory = null!;
        private Button btnCustomers = null!;
        private Button btnSales = null!;
        private Button btnSettings = null!;
        private Panel panelMenu = null!;
        private Panel panelContent = null!;
        private Label lblStatus = null!;
        private Label lblSync = null!;
        private Timer syncTimer = null!;
        
        public MainForm()
        {
            InitializeComponent();
            SetupEventHandlers();
            UpdateSyncStatus();
        }
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Configuracion del formulario principal
            this.Text = "SalvadoreX POS - Sistema de Punto de Venta";
            this.Size = new Size(1200, 800);
            this.MinimumSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.Font = new Font("Segoe UI", 10F);
            
            // Panel del menu lateral
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(10)
            };
            
            // Logo/Titulo
            var lblLogo = new Label
            {
                Text = "SalvadoreX",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(180, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            var lblSubtitle = new Label
            {
                Text = "Punto de Venta",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(20, 55),
                Size = new Size(180, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            
            // Botones del menu
            int buttonY = 100;
            int buttonHeight = 50;
            int buttonSpacing = 10;
            
            btnPOS = CreateMenuButton("   Punto de Venta", buttonY);
            btnPOS.BackColor = Color.FromArgb(59, 130, 246);
            buttonY += buttonHeight + buttonSpacing;
            
            btnInventory = CreateMenuButton("   Inventario", buttonY);
            buttonY += buttonHeight + buttonSpacing;
            
            btnCustomers = CreateMenuButton("   Clientes", buttonY);
            buttonY += buttonHeight + buttonSpacing;
            
            btnSales = CreateMenuButton("   Historial Ventas", buttonY);
            buttonY += buttonHeight + buttonSpacing;
            
            btnSettings = CreateMenuButton("   Configuracion", buttonY);
            
            // Estado de sincronizacion
            lblSync = new Label
            {
                Text = "Verificando conexion...",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(15, 680),
                Size = new Size(190, 40),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            panelMenu.Controls.AddRange(new Control[] { lblLogo, lblSubtitle, btnPOS, btnInventory, btnCustomers, btnSales, btnSettings, lblSync });
            
            // Panel de contenido
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 250),
                Padding = new Padding(20)
            };
            
            // Barra de estado
            lblStatus = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Text = "  Listo",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F)
            };
            
            // Timer para actualizar estado de sincronizacion
            syncTimer = new Timer { Interval = 5000 };
            syncTimer.Tick += (s, e) => UpdateSyncStatus();
            syncTimer.Start();
            
            this.Controls.Add(panelContent);
            this.Controls.Add(panelMenu);
            this.Controls.Add(lblStatus);
            
            this.ResumeLayout(false);
            
            // Mostrar POS por defecto
            ShowPOS();
        }
        
        private Button CreateMenuButton(string text, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(10, y),
                Size = new Size(200, 50),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(71, 85, 105) }
            };
        }
        
        private void SetupEventHandlers()
        {
            btnPOS.Click += (s, e) => ShowPOS();
            btnInventory.Click += (s, e) => ShowInventory();
            btnCustomers.Click += (s, e) => ShowCustomers();
            btnSales.Click += (s, e) => ShowSalesHistory();
            btnSettings.Click += (s, e) => ShowSettings();
            
            SyncService.Instance.SyncStatusChanged += OnSyncStatusChanged;
            SyncService.Instance.ConnectionStatusChanged += OnConnectionStatusChanged;
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
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            syncTimer?.Stop();
            base.OnFormClosing(e);
        }
    }
}
