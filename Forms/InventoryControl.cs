using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Forms
{
    public partial class InventoryControl : UserControl
    {
        private readonly ProductRepository _productRepo;
        private readonly CategoryRepository _categoryRepo;
        
        private List<Product> _products = new();
        private List<Category> _categories = new();
        
        public InventoryControl()
        {
            _productRepo = new ProductRepository();
            _categoryRepo = new CategoryRepository();
            
            InitializeComponent();
        }
        
        private void InventoryControl_Load(object sender, EventArgs e)
        {
            LoadData();
            ApplyGridStyles();
        }
        
        private void ApplyGridStyles()
        {
            gridProducts.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft
            };
            
            gridProducts.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.Black
            };
            
            gridProducts.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
        }
        
        private void LoadData()
        {
            _categories = _categoryRepo.GetAll();
            _products = _productRepo.GetAll(activeOnly: false);
            
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Todas las categorias");
            foreach (var cat in _categories)
            {
                cmbCategory.Items.Add(cat);
            }
            cmbCategory.SelectedIndex = 0;
            cmbCategory.DisplayMember = "Name";
            
            RefreshGrid();
        }
        
        private void RefreshGrid()
        {
            var searchText = txtSearch.Text.ToLower();
            var selectedCategory = cmbCategory.SelectedItem as Category;
            
            var filtered = _products.AsEnumerable();
            
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(p => 
                    p.Name.ToLower().Contains(searchText) ||
                    (p.Sku?.ToLower().Contains(searchText) ?? false) ||
                    (p.Barcode?.ToLower().Contains(searchText) ?? false)
                );
            }
            
            if (selectedCategory != null)
            {
                filtered = filtered.Where(p => p.CategoryId == selectedCategory.Id);
            }
            
            gridProducts.DataSource = null;
            gridProducts.DataSource = filtered.ToList();
        }
        
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        
        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using var form = new ProductFormDialog(_categories);
            if (form.ShowDialog() == DialogResult.OK && form.Product != null)
            {
                _productRepo.Insert(form.Product);
                _products = _productRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Producto agregado exitosamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (gridProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para editar", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var product = gridProducts.SelectedRows[0].DataBoundItem as Product;
            if (product == null) return;
            
            using var form = new ProductFormDialog(_categories, product);
            if (form.ShowDialog() == DialogResult.OK && form.Product != null)
            {
                _productRepo.Update(form.Product);
                _products = _productRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Producto actualizado exitosamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (gridProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var product = gridProducts.SelectedRows[0].DataBoundItem as Product;
            if (product == null) return;
            
            var result = MessageBox.Show(
                $"Esta seguro de eliminar el producto '{product.Name}'?",
                "Confirmar Eliminacion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            
            if (result == DialogResult.Yes)
            {
                _productRepo.Delete(product.Id);
                _products = _productRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Producto eliminado", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void GridProducts_DoubleClick(object sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }
    }
    
    public class ProductFormDialog : Form
    {
        public Product? Product { get; private set; }
        
        private TextBox txtName = null!;
        private TextBox txtDescription = null!;
        private TextBox txtSku = null!;
        private TextBox txtBarcode = null!;
        private NumericUpDown numPrice = null!;
        private NumericUpDown numCost = null!;
        private NumericUpDown numStock = null!;
        private NumericUpDown numMinStock = null!;
        private ComboBox cmbCategory = null!;
        private CheckBox chkActive = null!;
        private CheckBox chkAvailablePos = null!;
        private CheckBox chkAvailableDigitalMenu = null!;
        
        private readonly List<Category> _categories;
        private readonly bool _isEdit;
        
        public ProductFormDialog(List<Category> categories, Product? product = null)
        {
            _categories = categories;
            _isEdit = product != null;
            Product = product ?? new Product();
            
            InitializeComponent();
            LoadProduct();
        }
        
        private void InitializeComponent()
        {
            this.Text = _isEdit ? "Editar Producto" : "Nuevo Producto";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            
            var y = 20;
            var inputX = 130;
            var inputWidth = 330;
            var rowHeight = 40;
            
            AddLabel("Nombre:", y);
            txtName = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Descripcion:", y);
            txtDescription = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("SKU:", y);
            txtSku = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Codigo Barras:", y);
            txtBarcode = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Categoria:", y);
            cmbCategory = new ComboBox
            {
                Location = new Point(inputX, y),
                Size = new Size(inputWidth, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = "Name"
            };
            cmbCategory.Items.Add("-- Sin Categoria --");
            foreach (var cat in _categories)
            {
                cmbCategory.Items.Add(cat);
            }
            this.Controls.Add(cmbCategory);
            y += rowHeight;
            
            AddLabel("Precio:", y);
            numPrice = AddNumericUpDown(inputX, y, 150);
            numPrice.DecimalPlaces = 2;
            numPrice.Maximum = 999999;
            y += rowHeight;
            
            AddLabel("Costo:", y);
            numCost = AddNumericUpDown(inputX, y, 150);
            numCost.DecimalPlaces = 2;
            numCost.Maximum = 999999;
            y += rowHeight;
            
            AddLabel("Stock:", y);
            numStock = AddNumericUpDown(inputX, y, 100);
            numStock.Maximum = 999999;
            y += rowHeight;
            
            AddLabel("Stock Minimo:", y);
            numMinStock = AddNumericUpDown(inputX, y, 100);
            numMinStock.Maximum = 999999;
            y += rowHeight;
            
            chkActive = new CheckBox
            {
                Text = "Activo",
                Location = new Point(inputX, y),
                Checked = true,
                AutoSize = true
            };
            this.Controls.Add(chkActive);
            y += rowHeight;
            
            chkAvailablePos = new CheckBox
            {
                Text = "Disponible en POS",
                Location = new Point(inputX, y),
                Checked = true,
                AutoSize = true
            };
            this.Controls.Add(chkAvailablePos);
            y += rowHeight;
            
            chkAvailableDigitalMenu = new CheckBox
            {
                Text = "Disponible en Menu Digital",
                Location = new Point(inputX, y),
                Checked = true,
                AutoSize = true
            };
            this.Controls.Add(chkAvailableDigitalMenu);
            y += rowHeight + 20;
            
            var btnSave = new Button
            {
                Text = "Guardar",
                Location = new Point(inputX, y),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(34, 197, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            
            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(inputX + 110, y),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            
            this.Controls.AddRange(new Control[] { btnSave, btnCancel });
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }
        
        private Label AddLabel(string text, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(20, y + 5),
                AutoSize = true
            };
            this.Controls.Add(label);
            return label;
        }
        
        private TextBox AddTextBox(int x, int y, int width)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 30)
            };
            this.Controls.Add(textBox);
            return textBox;
        }
        
        private NumericUpDown AddNumericUpDown(int x, int y, int width)
        {
            var numericUpDown = new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(width, 30)
            };
            this.Controls.Add(numericUpDown);
            return numericUpDown;
        }
        
        private void LoadProduct()
        {
            if (Product == null) return;
            
            txtName.Text = Product.Name;
            txtDescription.Text = Product.Description;
            txtSku.Text = Product.Sku;
            txtBarcode.Text = Product.Barcode;
            numPrice.Value = Product.Price;
            numCost.Value = Product.Cost;
            numStock.Value = Product.Stock;
            numMinStock.Value = Product.MinStock;
            chkActive.Checked = Product.Active;
            chkAvailablePos.Checked = Product.AvailablePos;
            chkAvailableDigitalMenu.Checked = Product.AvailableDigitalMenu;
            
            if (!string.IsNullOrEmpty(Product.CategoryId))
            {
                var cat = _categories.FirstOrDefault(c => c.Id == Product.CategoryId);
                if (cat != null)
                {
                    cmbCategory.SelectedItem = cat;
                }
            }
            else
            {
                cmbCategory.SelectedIndex = 0;
            }
        }
        
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre es requerido", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            
            Product!.Name = txtName.Text.Trim();
            Product.Description = txtDescription.Text.Trim();
            Product.Sku = txtSku.Text.Trim();
            Product.Barcode = txtBarcode.Text.Trim();
            Product.Price = numPrice.Value;
            Product.Cost = numCost.Value;
            Product.Stock = (int)numStock.Value;
            Product.MinStock = (int)numMinStock.Value;
            Product.Active = chkActive.Checked;
            Product.AvailablePos = chkAvailablePos.Checked;
            Product.AvailableDigitalMenu = chkAvailableDigitalMenu.Checked;
            
            var selectedCat = cmbCategory.SelectedItem as Category;
            Product.CategoryId = selectedCat?.Id;
        }
    }
}
