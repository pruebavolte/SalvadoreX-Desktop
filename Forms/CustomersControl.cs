using SalvadoreXDesktop.Data;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Forms
{
    public class CustomersControl : UserControl
    {
        private readonly CustomerRepository _customerRepo;
        
        private DataGridView gridCustomers = null!;
        private TextBox txtSearch = null!;
        private Button btnAdd = null!;
        private Button btnEdit = null!;
        private Button btnDelete = null!;
        
        private List<Customer> _customers = new();
        
        public CustomersControl()
        {
            _customerRepo = new CustomerRepository();
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
                Height = 60
            };
            
            var lblTitle = new Label
            {
                Text = "Clientes",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(0, 5),
                AutoSize = true
            };
            
            txtSearch = new TextBox
            {
                Location = new Point(200, 10),
                Size = new Size(250, 30),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Buscar cliente..."
            };
            txtSearch.TextChanged += (s, e) => RefreshGrid();
            
            btnAdd = CreateButton("+ Nuevo", Color.FromArgb(34, 197, 94), 470);
            btnAdd.Click += BtnAdd_Click;
            
            btnEdit = CreateButton("Editar", Color.FromArgb(59, 130, 246), 575);
            btnEdit.Click += BtnEdit_Click;
            
            btnDelete = CreateButton("Eliminar", Color.FromArgb(239, 68, 68), 660);
            btnDelete.Click += BtnDelete_Click;
            
            panelTop.Controls.AddRange(new Control[] { lblTitle, txtSearch, btnAdd, btnEdit, btnDelete });
            
            // Grid
            gridCustomers = new DataGridView
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
            
            gridCustomers.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Nombre", DataPropertyName = "Name", Width = 200 },
                new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", DataPropertyName = "Email", Width = 200 },
                new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Telefono", DataPropertyName = "Phone", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "Rfc", HeaderText = "RFC", DataPropertyName = "Rfc", Width = 130 },
                new DataGridViewTextBoxColumn { Name = "CreditLimit", HeaderText = "Limite Credito", DataPropertyName = "CreditLimit", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } },
                new DataGridViewTextBoxColumn { Name = "CurrentCredit", HeaderText = "Credito Usado", DataPropertyName = "CurrentCredit", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } },
                new DataGridViewTextBoxColumn { Name = "LoyaltyPoints", HeaderText = "Puntos", DataPropertyName = "LoyaltyPoints", Width = 80 }
            });
            
            gridCustomers.DoubleClick += (s, e) => BtnEdit_Click(s, e);
            ApplyGridStyles(gridCustomers);
            
            this.Controls.Add(gridCustomers);
            this.Controls.Add(panelTop);
        }
        
        private Button CreateButton(string text, Color color, int x)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 8),
                Size = new Size(100, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
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
            _customers = _customerRepo.GetAll(activeOnly: false);
            RefreshGrid();
        }
        
        private void RefreshGrid()
        {
            var searchText = txtSearch.Text.ToLower();
            
            var filtered = _customers.AsEnumerable();
            
            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(c =>
                    c.Name.ToLower().Contains(searchText) ||
                    (c.Email?.ToLower().Contains(searchText) ?? false) ||
                    (c.Phone?.ToLower().Contains(searchText) ?? false)
                );
            }
            
            gridCustomers.DataSource = null;
            gridCustomers.DataSource = filtered.ToList();
        }
        
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var form = new CustomerFormDialog();
            if (form.ShowDialog() == DialogResult.OK && form.Customer != null)
            {
                _customerRepo.Insert(form.Customer);
                _customers = _customerRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Cliente agregado exitosamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (gridCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para editar", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var customer = gridCustomers.SelectedRows[0].DataBoundItem as Customer;
            if (customer == null) return;
            
            using var form = new CustomerFormDialog(customer);
            if (form.ShowDialog() == DialogResult.OK && form.Customer != null)
            {
                _customerRepo.Update(form.Customer);
                _customers = _customerRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Cliente actualizado exitosamente", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (gridCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para eliminar", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var customer = gridCustomers.SelectedRows[0].DataBoundItem as Customer;
            if (customer == null) return;
            
            var result = MessageBox.Show(
                $"Esta seguro de eliminar al cliente '{customer.Name}'?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            
            if (result == DialogResult.Yes)
            {
                _customerRepo.Delete(customer.Id);
                _customers = _customerRepo.GetAll(activeOnly: false);
                RefreshGrid();
                MessageBox.Show("Cliente eliminado", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    
    public class CustomerFormDialog : Form
    {
        public Customer? Customer { get; private set; }
        
        private TextBox txtName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtAddress = null!;
        private TextBox txtRfc = null!;
        private NumericUpDown numCreditLimit = null!;
        private TextBox txtNotes = null!;
        
        private readonly bool _isEdit;
        
        public CustomerFormDialog(Customer? customer = null)
        {
            _isEdit = customer != null;
            Customer = customer ?? new Customer();
            InitializeComponent();
            LoadCustomer();
        }
        
        private void InitializeComponent()
        {
            this.Text = _isEdit ? "Editar Cliente" : "Nuevo Cliente";
            this.Size = new Size(450, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);
            
            var y = 20;
            var inputX = 120;
            var inputWidth = 290;
            var rowHeight = 40;
            
            AddLabel("Nombre:", y);
            txtName = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Email:", y);
            txtEmail = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Telefono:", y);
            txtPhone = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Direccion:", y);
            txtAddress = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("RFC:", y);
            txtRfc = AddTextBox(inputX, y, inputWidth);
            y += rowHeight;
            
            AddLabel("Limite Credito:", y);
            numCreditLimit = new NumericUpDown
            {
                Location = new Point(inputX, y),
                Size = new Size(150, 30),
                DecimalPlaces = 2,
                Maximum = 999999
            };
            this.Controls.Add(numCreditLimit);
            y += rowHeight;
            
            AddLabel("Notas:", y);
            txtNotes = new TextBox
            {
                Location = new Point(inputX, y),
                Size = new Size(inputWidth, 60),
                Multiline = true
            };
            this.Controls.Add(txtNotes);
            y += 80;
            
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
        
        private void AddLabel(string text, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(20, y + 5),
                AutoSize = true
            };
            this.Controls.Add(label);
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
        
        private void LoadCustomer()
        {
            if (Customer == null) return;
            
            txtName.Text = Customer.Name;
            txtEmail.Text = Customer.Email;
            txtPhone.Text = Customer.Phone;
            txtAddress.Text = Customer.Address;
            txtRfc.Text = Customer.Rfc;
            numCreditLimit.Value = Customer.CreditLimit;
            txtNotes.Text = Customer.Notes;
        }
        
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("El nombre es requerido", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
            
            Customer!.Name = txtName.Text.Trim();
            Customer.Email = txtEmail.Text.Trim();
            Customer.Phone = txtPhone.Text.Trim();
            Customer.Address = txtAddress.Text.Trim();
            Customer.Rfc = txtRfc.Text.Trim();
            Customer.CreditLimit = numCreditLimit.Value;
            Customer.Notes = txtNotes.Text.Trim();
        }
    }
}
