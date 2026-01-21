using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;
using SalvadoreXDesktop.Services;

namespace SalvadoreXDesktop.Forms
{
    public partial class POSControl : UserControl
    {
        private readonly ProductRepository _productRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly SaleRepository _saleRepo;
        private readonly PrintService _printService;
        
        private List<SaleItem> _cartItems = new();
        private List<Product> _allProducts = new();
        private List<Category> _categories = new();
        private Customer? _selectedCustomer;
        
        public POSControl()
        {
            _productRepo = new ProductRepository();
            _categoryRepo = new CategoryRepository();
            _customerRepo = new CustomerRepository();
            _saleRepo = new SaleRepository();
            _printService = new PrintService();
            
            InitializeComponent();
        }
        
        private void POSControl_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void LoadData()
        {
            _categories = _categoryRepo.GetAll();
            
            panelCategories.Controls.Clear();
            var btnAll = CreateCategoryButton("Todos", null, Color.FromArgb(107, 114, 128));
            btnAll.Click += (s, e) => FilterProducts(null);
            panelCategories.Controls.Add(btnAll);
            
            foreach (var category in _categories)
            {
                var color = ColorTranslator.FromHtml(category.Color ?? "#3B82F6");
                var btn = CreateCategoryButton(category.Name, category.Id, color);
                btn.Click += (s, e) => FilterProducts(category.Id);
                panelCategories.Controls.Add(btn);
            }
            
            _allProducts = _productRepo.GetAll();
            DisplayProducts(_allProducts);
            
            var customers = _customerRepo.GetAll();
            cmbCustomer.Items.Clear();
            cmbCustomer.Items.Add("-- Publico General --");
            foreach (var customer in customers)
            {
                cmbCustomer.Items.Add(customer);
            }
            cmbCustomer.SelectedIndex = 0;
            cmbCustomer.DisplayMember = "Name";
        }
        
        private Button CreateCategoryButton(string name, string? id, Color color)
        {
            return new Button
            {
                Text = name,
                Tag = id,
                Size = new Size(100, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(3)
            };
        }
        
        private void FilterProducts(string? categoryId)
        {
            var products = categoryId == null 
                ? _allProducts 
                : _allProducts.Where(p => p.CategoryId == categoryId).ToList();
            DisplayProducts(products);
        }
        
        private void DisplayProducts(List<Product> products)
        {
            panelProducts.Controls.Clear();
            
            foreach (var product in products.Where(p => p.AvailablePos))
            {
                var btn = new Button
                {
                    Text = $"{product.Name}\n${product.Price:N2}",
                    Tag = product,
                    Size = new Size(130, 80),
                    BackColor = Color.FromArgb(59, 130, 246),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(5),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => AddToCart(product);
                
                panelProducts.Controls.Add(btn);
            }
        }
        
        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var barcode = txtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(barcode))
                {
                    var product = _productRepo.GetByBarcode(barcode);
                    if (product != null)
                    {
                        AddToCart(product);
                    }
                    else
                    {
                        var products = _productRepo.Search(barcode);
                        if (products.Count == 1)
                        {
                            AddToCart(products[0]);
                        }
                        else if (products.Count > 1)
                        {
                            DisplayProducts(products);
                        }
                        else
                        {
                            MessageBox.Show("Producto no encontrado", "Busqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                txtBarcode.Clear();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        
        private void AddToCart(Product product)
        {
            var existingItem = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            
            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.Total = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                var item = new SaleItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = 1,
                    Total = product.Price
                };
                _cartItems.Add(item);
            }
            
            RefreshCart();
            txtBarcode.Focus();
        }
        
        private void RefreshCart()
        {
            listCart.Items.Clear();
            
            foreach (var item in _cartItems)
            {
                var lvi = new ListViewItem(item.ProductName);
                lvi.SubItems.Add(item.Quantity.ToString());
                lvi.SubItems.Add($"${item.UnitPrice:N2}");
                lvi.SubItems.Add($"${item.Total:N2}");
                lvi.Tag = item;
                listCart.Items.Add(lvi);
            }
            
            UpdateTotals();
        }
        
        private void UpdateTotals()
        {
            var subtotal = _cartItems.Sum(i => i.Total);
            var tax = subtotal * 0.16m;
            var total = subtotal + tax;
            
            lblSubtotal.Text = $"Subtotal: ${subtotal:N2}";
            lblTax.Text = $"IVA (16%): ${tax:N2}";
            lblTotal.Text = $"TOTAL: ${total:N2}";
        }
        
        private void ListCart_KeyDown(object sender, KeyEventArgs e)
        {
            if (listCart.SelectedItems.Count > 0)
            {
                var item = listCart.SelectedItems[0].Tag as SaleItem;
                if (item == null) return;
                
                if (e.KeyCode == Keys.Delete)
                {
                    _cartItems.Remove(item);
                    RefreshCart();
                }
                else if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    item.Quantity++;
                    item.Total = item.Quantity * item.UnitPrice;
                    RefreshCart();
                }
                else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        item.Total = item.Quantity * item.UnitPrice;
                        RefreshCart();
                    }
                }
            }
        }
        
        private void ListCart_DoubleClick(object sender, EventArgs e)
        {
            if (listCart.SelectedItems.Count > 0)
            {
                var item = listCart.SelectedItems[0].Tag as SaleItem;
                if (item == null) return;
                
                var input = Microsoft.VisualBasic.Interaction.InputBox(
                    "Ingrese la cantidad:", 
                    "Modificar Cantidad", 
                    item.Quantity.ToString()
                );
                
                if (int.TryParse(input, out int newQty) && newQty > 0)
                {
                    item.Quantity = newQty;
                    item.Total = item.Quantity * item.UnitPrice;
                    RefreshCart();
                }
            }
        }
        
        private void CmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedCustomer = cmbCustomer.SelectedItem as Customer;
        }
        
        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            ClearCart();
        }
        
        private void ClearCart()
        {
            if (_cartItems.Count > 0)
            {
                var result = MessageBox.Show("Esta seguro de limpiar el carrito?", "Confirmar", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _cartItems.Clear();
                    RefreshCart();
                }
            }
        }
        
        private void BtnCash_Click(object sender, EventArgs e)
        {
            ProcessSale("cash");
        }
        
        private void BtnCard_Click(object sender, EventArgs e)
        {
            ProcessSale("card");
        }
        
        private void ProcessSale(string paymentMethod)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("El carrito esta vacio", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var subtotal = _cartItems.Sum(i => i.Total);
            var tax = subtotal * 0.16m;
            var total = subtotal + tax;
            
            var sale = new Sale
            {
                CustomerId = _selectedCustomer?.Id,
                CustomerName = _selectedCustomer?.Name ?? "Publico General",
                Subtotal = subtotal,
                Tax = tax,
                Total = total,
                PaymentMethod = paymentMethod,
                Status = "completed"
            };
            
            try
            {
                _saleRepo.Insert(sale, _cartItems.ToList());
                
                var printResult = MessageBox.Show(
                    $"Venta completada!\n\nTotal: ${total:N2}\nRecibo: {sale.ReceiptNumber}\n\nDesea imprimir el ticket?",
                    "Venta Exitosa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information
                );
                
                if (printResult == DialogResult.Yes)
                {
                    _printService.PrintReceipt(sale, _cartItems.ToList());
                }
                
                _cartItems.Clear();
                RefreshCart();
                cmbCustomer.SelectedIndex = 0;
                
                _allProducts = _productRepo.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                ProcessSale("cash");
                return true;
            }
            else if (keyData == Keys.F2)
            {
                ProcessSale("card");
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                ClearCart();
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
