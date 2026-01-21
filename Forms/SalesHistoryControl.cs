using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public partial class SalesHistoryControl : UserControl
    {
        private readonly SaleRepository _saleRepo;
        private readonly PrintService _printService;
        
        private List<Sale> _sales = new();
        
        public SalesHistoryControl()
        {
            _saleRepo = new SaleRepository();
            _printService = new PrintService();
            
            InitializeComponent();
        }
        
        private void SalesHistoryControl_Load(object sender, EventArgs e)
        {
            ApplyGridStyles();
            LoadData();
        }
        
        private void ApplyGridStyles()
        {
            gridSales.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            
            gridSales.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.Black
            };
            
            gridSales.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
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
            
            var total = _sales.Where(s => s.Status == "completed").Sum(s => s.Total);
            var count = _sales.Count(s => s.Status == "completed");
            
            lblTotalSales.Text = $"Ventas: {count}";
            lblTotalAmount.Text = $"Total: ${total:N2}";
        }
        
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void GridSales_DoubleClick(object sender, EventArgs e)
        {
            if (gridSales.SelectedRows.Count == 0) return;
            
            var sale = gridSales.SelectedRows[0].DataBoundItem as Sale;
            if (sale == null) return;
            
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
        
        private void BtnPrint_Click(object sender, EventArgs e)
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
