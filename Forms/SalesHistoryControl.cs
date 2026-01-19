using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public class SalesHistoryControl : UserControl
    {
        private readonly SaleRepository _saleRepo;
        private readonly PrintService _printService;
        
        private DataGridView gridSales = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private Button btnFilter = null!;
        private Button btnPrint = null!;
        private Label lblTotalSales = null!;
        private Label lblTotalAmount = null!;
        
        private List<Sale> _sales = new();
        
        public SalesHistoryControl()
        {
            _saleRepo = new SaleRepository();
            _printService = new PrintService();
            
            InitializeComponent();
            LoadData();
        }
        
        private void InitializeComponent()
        {
            this.BackColor = Color.White;
            this.Padding = new Padding(20);
            
            // Panel superior
            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100
            };
            
            var lblTitle = new Label
            {
                Text = "Historial de Ventas",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 5),
                AutoSize = true
            };
            
            // Filtros de fecha
            var lblFrom = new Label { Text = "Desde:", Location = new Point(0, 55), AutoSize = true };
            dtpFrom = new DateTimePicker
            {
                Location = new Point(60, 50),
                Size = new Size(150, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddDays(-7)
            };
            
            var lblTo = new Label { Text = "Hasta:", Location = new Point(230, 55), AutoSize = true };
            dtpTo = new DateTimePicker
            {
                Location = new Point(290, 50),
                Size = new Size(150, 30),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            
            btnFilter = new Button
            {
                Text = "Filtrar",
                Location = new Point(460, 48),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += (s, e) => LoadData();
            
            btnPrint = new Button
            {
                Text = "Reimprimir",
                Location = new Point(550, 48),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Click += BtnPrint_Click;
            
            // Resumen
            lblTotalSales = new Label
            {
                Text = "Ventas: 0",
                Location = new Point(680, 50),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            
            lblTotalAmount = new Label
            {
                Text = "Total: $0.00",
                Location = new Point(680, 72),
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 197, 94)
            };
            
            panelTop.Controls.AddRange(new Control[] 
            { 
                lblTitle, lblFrom, dtpFrom, lblTo, dtpTo, btnFilter, btnPrint, 
                lblTotalSales, lblTotalAmount 
            });
            
            // Grid
            gridSales = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10F),
                ColumnHeadersHeight = 40,
                RowTemplate = { Height = 35 }
            };
            
            gridSales.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "ReceiptNumber", HeaderText = "# Recibo", DataPropertyName = "ReceiptNumber", Width = 140 },
                new DataGridViewTextBoxColumn { Name = "CreatedAt", HeaderText = "Fecha", DataPropertyName = "CreatedAt", Width = 160, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } },
                new DataGridViewTextBoxColumn { Name = "CustomerName", HeaderText = "Cliente", DataPropertyName = "CustomerName", Width = 180 },
                new DataGridViewTextBoxColumn { Name = "Subtotal", HeaderText = "Subtotal", DataPropertyName = "Subtotal", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } },
                new DataGridViewTextBoxColumn { Name = "Tax", HeaderText = "IVA", DataPropertyName = "Tax", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } },
                new DataGridViewTextBoxColumn { Name = "Discount", HeaderText = "Descuento", DataPropertyName = "Discount", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } },
                new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", DataPropertyName = "Total", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Font = new Font("Segoe UI", 10F, FontStyle.Bold) } },
                new DataGridViewTextBoxColumn { Name = "PaymentMethod", HeaderText = "Pago", DataPropertyName = "PaymentMethod", Width = 100 },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Estado", DataPropertyName = "Status", Width = 80 }
            });
            
            ApplyGridStyles(gridSales);
            gridSales.DoubleClick += GridSales_DoubleClick;
            
            this.Controls.Add(gridSales);
            this.Controls.Add(panelTop);
        }
        
        private void ApplyGridStyles(DataGridView grid)
        {
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.Black
            };
            
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
        }
        
        private void LoadData()
        {
            var from = dtpFrom.Value.Date;
            var to = dtpTo.Value.Date.AddDays(1);
            
            _sales = _saleRepo.GetAll(from, to);
            
            gridSales.DataSource = null;
            gridSales.DataSource = _sales;
            
            // Actualizar resumen
            var total = _sales.Where(s => s.Status == "completed").Sum(s => s.Total);
            var count = _sales.Count(s => s.Status == "completed");
            
            lblTotalSales.Text = $"Ventas: {count}";
            lblTotalAmount.Text = $"Total: ${total:N2}";
        }
        
        private void GridSales_DoubleClick(object? sender, EventArgs e)
        {
            if (gridSales.SelectedRows.Count == 0) return;
            
            var sale = gridSales.SelectedRows[0].DataBoundItem as Sale;
            if (sale == null) return;
            
            // Cargar detalles de la venta
            var fullSale = _saleRepo.GetById(sale.Id);
            if (fullSale == null) return;
            
            var details = $"Recibo: {fullSale.ReceiptNumber}\n" +
                         $"Fecha: {fullSale.CreatedAt:dd/MM/yyyy HH:mm}\n" +
                         $"Cliente: {fullSale.CustomerName}\n\n" +
                         "Items:\n";
            
            foreach (var item in fullSale.Items)
            {
                details += $"  {item.Quantity}x {item.ProductName} = ${item.Total:N2}\n";
            }
            
            details += $"\nSubtotal: ${fullSale.Subtotal:N2}\n" +
                      $"IVA: ${fullSale.Tax:N2}\n" +
                      $"Total: ${fullSale.Total:N2}\n" +
                      $"Pago: {fullSale.PaymentMethod}";
            
            MessageBox.Show(details, "Detalle de Venta", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void BtnPrint_Click(object? sender, EventArgs e)
        {
            if (gridSales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una venta para reimprimir", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var sale = gridSales.SelectedRows[0].DataBoundItem as Sale;
            if (sale == null) return;
            
            var fullSale = _saleRepo.GetById(sale.Id);
            if (fullSale == null) return;
            
            _printService.PrintReceipt(fullSale, fullSale.Items);
        }
    }
}
