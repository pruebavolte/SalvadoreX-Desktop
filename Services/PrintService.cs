using System.Drawing.Printing;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Services
{
    public class PrintService
    {
        private Sale? _saleToPrint;
        private List<SaleItem>? _itemsToPrint;
        
        public void PrintReceipt(Sale sale, List<SaleItem> items)
        {
            _saleToPrint = sale;
            _itemsToPrint = items;
            
            var printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            
            try
            {
                // Mostrar dialogo de impresion
                var printDialog = new PrintDialog
                {
                    Document = printDocument
                };
                
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_saleToPrint == null || _itemsToPrint == null || e.Graphics == null) return;
            
            var graphics = e.Graphics;
            var font = new Font("Consolas", 10);
            var boldFont = new Font("Consolas", 10, FontStyle.Bold);
            var titleFont = new Font("Consolas", 14, FontStyle.Bold);
            
            float y = 10;
            float lineHeight = 20;
            float margin = 10;
            float width = 280; // Ancho tipico de ticket
            
            // Titulo
            graphics.DrawString("SALVADOREX POS", titleFont, Brushes.Black, margin, y);
            y += lineHeight * 1.5f;
            
            // Linea separadora
            graphics.DrawLine(Pens.Black, margin, y, width, y);
            y += 10;
            
            // Numero de recibo
            graphics.DrawString($"Recibo: {_saleToPrint.ReceiptNumber}", boldFont, Brushes.Black, margin, y);
            y += lineHeight;
            
            // Fecha
            graphics.DrawString($"Fecha: {_saleToPrint.CreatedAt:dd/MM/yyyy HH:mm}", font, Brushes.Black, margin, y);
            y += lineHeight;
            
            // Cliente
            if (!string.IsNullOrEmpty(_saleToPrint.CustomerName))
            {
                graphics.DrawString($"Cliente: {_saleToPrint.CustomerName}", font, Brushes.Black, margin, y);
                y += lineHeight;
            }
            
            // Linea separadora
            y += 5;
            graphics.DrawLine(Pens.Black, margin, y, width, y);
            y += 10;
            
            // Encabezado de productos
            graphics.DrawString("Producto", boldFont, Brushes.Black, margin, y);
            graphics.DrawString("Cant", boldFont, Brushes.Black, 150, y);
            graphics.DrawString("Total", boldFont, Brushes.Black, 200, y);
            y += lineHeight;
            
            // Items
            foreach (var item in _itemsToPrint)
            {
                graphics.DrawString(TruncateString(item.ProductName, 18), font, Brushes.Black, margin, y);
                graphics.DrawString(item.Quantity.ToString(), font, Brushes.Black, 150, y);
                graphics.DrawString($"${item.Total:N2}", font, Brushes.Black, 200, y);
                y += lineHeight;
            }
            
            // Linea separadora
            y += 5;
            graphics.DrawLine(Pens.Black, margin, y, width, y);
            y += 10;
            
            // Totales
            graphics.DrawString($"Subtotal:", font, Brushes.Black, margin, y);
            graphics.DrawString($"${_saleToPrint.Subtotal:N2}", font, Brushes.Black, 200, y);
            y += lineHeight;
            
            if (_saleToPrint.Discount > 0)
            {
                graphics.DrawString($"Descuento:", font, Brushes.Black, margin, y);
                graphics.DrawString($"-${_saleToPrint.Discount:N2}", font, Brushes.Black, 200, y);
                y += lineHeight;
            }
            
            if (_saleToPrint.Tax > 0)
            {
                graphics.DrawString($"IVA:", font, Brushes.Black, margin, y);
                graphics.DrawString($"${_saleToPrint.Tax:N2}", font, Brushes.Black, 200, y);
                y += lineHeight;
            }
            
            // Total
            graphics.DrawString($"TOTAL:", boldFont, Brushes.Black, margin, y);
            graphics.DrawString($"${_saleToPrint.Total:N2}", boldFont, Brushes.Black, 200, y);
            y += lineHeight * 1.5f;
            
            // Metodo de pago
            var paymentMethodText = _saleToPrint.PaymentMethod switch
            {
                "cash" => "Efectivo",
                "card" => "Tarjeta",
                "transfer" => "Transferencia",
                _ => _saleToPrint.PaymentMethod
            };
            graphics.DrawString($"Pago: {paymentMethodText}", font, Brushes.Black, margin, y);
            y += lineHeight * 2;
            
            // Mensaje de agradecimiento
            graphics.DrawString("Gracias por su compra!", font, Brushes.Black, margin + 40, y);
            
            e.HasMorePages = false;
        }
        
        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength - 2) + "..";
        }
    }
}
