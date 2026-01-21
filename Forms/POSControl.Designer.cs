namespace SalvadoreXDesktop.Forms
{
    partial class POSControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.panelCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.panelRight = new System.Windows.Forms.Panel();
            this.listCart = new System.Windows.Forms.ListView();
            this.columnProducto = new System.Windows.Forms.ColumnHeader();
            this.columnCant = new System.Windows.Forms.ColumnHeader();
            this.columnPrecio = new System.Windows.Forms.ColumnHeader();
            this.columnTotal = new System.Windows.Forms.ColumnHeader();
            this.panelTotals = new System.Windows.Forms.Panel();
            this.btnClearCart = new System.Windows.Forms.Button();
            this.btnCard = new System.Windows.Forms.Button();
            this.btnCash = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTax = new System.Windows.Forms.Label();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.panelCustomer = new System.Windows.Forms.Panel();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblCartTitle = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelTotals.SuspendLayout();
            this.panelCustomer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelProducts);
            this.panelLeft.Controls.Add(this.panelCategories);
            this.panelLeft.Controls.Add(this.panelSearch);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(10);
            this.panelLeft.Size = new System.Drawing.Size(600, 600);
            this.panelLeft.TabIndex = 0;
            // 
            // panelProducts
            // 
            this.panelProducts.AutoScroll = true;
            this.panelProducts.BackColor = System.Drawing.Color.White;
            this.panelProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProducts.Location = new System.Drawing.Point(10, 120);
            this.panelProducts.Name = "panelProducts";
            this.panelProducts.Padding = new System.Windows.Forms.Padding(10);
            this.panelProducts.Size = new System.Drawing.Size(580, 470);
            this.panelProducts.TabIndex = 2;
            // 
            // panelCategories
            // 
            this.panelCategories.AutoScroll = true;
            this.panelCategories.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCategories.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.panelCategories.Location = new System.Drawing.Point(10, 60);
            this.panelCategories.Name = "panelCategories";
            this.panelCategories.Padding = new System.Windows.Forms.Padding(5);
            this.panelCategories.Size = new System.Drawing.Size(580, 60);
            this.panelCategories.TabIndex = 1;
            this.panelCategories.WrapContents = false;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.txtBarcode);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(10, 10);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Padding = new System.Windows.Forms.Padding(5);
            this.panelSearch.Size = new System.Drawing.Size(580, 50);
            this.panelSearch.TabIndex = 0;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtBarcode.Location = new System.Drawing.Point(5, 5);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.PlaceholderText = "Escanear codigo de barras o buscar producto...";
            this.txtBarcode.Size = new System.Drawing.Size(570, 39);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBarcode_KeyDown);
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.White;
            this.panelRight.Controls.Add(this.listCart);
            this.panelRight.Controls.Add(this.panelTotals);
            this.panelRight.Controls.Add(this.panelCustomer);
            this.panelRight.Controls.Add(this.lblCartTitle);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(600, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Padding = new System.Windows.Forms.Padding(15);
            this.panelRight.Size = new System.Drawing.Size(400, 600);
            this.panelRight.TabIndex = 1;
            // 
            // listCart
            // 
            this.listCart.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnProducto,
            this.columnCant,
            this.columnPrecio,
            this.columnTotal});
            this.listCart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listCart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listCart.FullRowSelect = true;
            this.listCart.GridLines = true;
            this.listCart.Location = new System.Drawing.Point(15, 95);
            this.listCart.Name = "listCart";
            this.listCart.Size = new System.Drawing.Size(370, 310);
            this.listCart.TabIndex = 3;
            this.listCart.UseCompatibleStateImageBehavior = false;
            this.listCart.View = System.Windows.Forms.View.Details;
            this.listCart.DoubleClick += new System.EventHandler(this.ListCart_DoubleClick);
            this.listCart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListCart_KeyDown);
            // 
            // columnProducto
            // 
            this.columnProducto.Text = "Producto";
            this.columnProducto.Width = 150;
            // 
            // columnCant
            // 
            this.columnCant.Text = "Cant";
            this.columnCant.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnCant.Width = 50;
            // 
            // columnPrecio
            // 
            this.columnPrecio.Text = "Precio";
            this.columnPrecio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnPrecio.Width = 80;
            // 
            // columnTotal
            // 
            this.columnTotal.Text = "Total";
            this.columnTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnTotal.Width = 80;
            // 
            // panelTotals
            // 
            this.panelTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelTotals.Controls.Add(this.btnClearCart);
            this.panelTotals.Controls.Add(this.btnCard);
            this.panelTotals.Controls.Add(this.btnCash);
            this.panelTotals.Controls.Add(this.lblTotal);
            this.panelTotals.Controls.Add(this.lblTax);
            this.panelTotals.Controls.Add(this.lblSubtotal);
            this.panelTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTotals.Location = new System.Drawing.Point(15, 405);
            this.panelTotals.Name = "panelTotals";
            this.panelTotals.Size = new System.Drawing.Size(370, 180);
            this.panelTotals.TabIndex = 2;
            // 
            // btnClearCart
            // 
            this.btnClearCart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnClearCart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearCart.FlatAppearance.BorderSize = 0;
            this.btnClearCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearCart.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClearCart.ForeColor = System.Drawing.Color.White;
            this.btnClearCart.Location = new System.Drawing.Point(290, 115);
            this.btnClearCart.Name = "btnClearCart";
            this.btnClearCart.Size = new System.Drawing.Size(70, 50);
            this.btnClearCart.TabIndex = 5;
            this.btnClearCart.Text = "Limpiar";
            this.btnClearCart.UseVisualStyleBackColor = false;
            this.btnClearCart.Click += new System.EventHandler(this.BtnClearCart_Click);
            // 
            // btnCard
            // 
            this.btnCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCard.FlatAppearance.BorderSize = 0;
            this.btnCard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCard.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCard.ForeColor = System.Drawing.Color.White;
            this.btnCard.Location = new System.Drawing.Point(150, 115);
            this.btnCard.Name = "btnCard";
            this.btnCard.Size = new System.Drawing.Size(130, 50);
            this.btnCard.TabIndex = 4;
            this.btnCard.Text = "Tarjeta (F2)";
            this.btnCard.UseVisualStyleBackColor = false;
            this.btnCard.Click += new System.EventHandler(this.BtnCard_Click);
            // 
            // btnCash
            // 
            this.btnCash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnCash.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCash.FlatAppearance.BorderSize = 0;
            this.btnCash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCash.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCash.ForeColor = System.Drawing.Color.White;
            this.btnCash.Location = new System.Drawing.Point(10, 115);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(130, 50);
            this.btnCash.TabIndex = 3;
            this.btnCash.Text = "Efectivo (F1)";
            this.btnCash.UseVisualStyleBackColor = false;
            this.btnCash.Click += new System.EventHandler(this.BtnCash_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblTotal.Location = new System.Drawing.Point(10, 60);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(300, 45);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "TOTAL: $0.00";
            // 
            // lblTax
            // 
            this.lblTax.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTax.Location = new System.Drawing.Point(10, 35);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(300, 25);
            this.lblTax.TabIndex = 1;
            this.lblTax.Text = "IVA (16%): $0.00";
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtotal.Location = new System.Drawing.Point(10, 10);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(300, 25);
            this.lblSubtotal.TabIndex = 0;
            this.lblSubtotal.Text = "Subtotal: $0.00";
            // 
            // panelCustomer
            // 
            this.panelCustomer.Controls.Add(this.cmbCustomer);
            this.panelCustomer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCustomer.Location = new System.Drawing.Point(15, 55);
            this.panelCustomer.Name = "panelCustomer";
            this.panelCustomer.Size = new System.Drawing.Size(370, 40);
            this.panelCustomer.TabIndex = 1;
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(0, 0);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(370, 33);
            this.cmbCustomer.TabIndex = 0;
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.CmbCustomer_SelectedIndexChanged);
            // 
            // lblCartTitle
            // 
            this.lblCartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCartTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblCartTitle.Location = new System.Drawing.Point(15, 15);
            this.lblCartTitle.Name = "lblCartTitle";
            this.lblCartTitle.Size = new System.Drawing.Size(370, 40);
            this.lblCartTitle.TabIndex = 0;
            this.lblCartTitle.Text = "Carrito de Venta";
            // 
            // POSControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Name = "POSControl";
            this.Size = new System.Drawing.Size(1000, 600);
            this.Load += new System.EventHandler(this.POSControl_Load);
            this.panelLeft.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelTotals.ResumeLayout(false);
            this.panelCustomer.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.FlowLayoutPanel panelProducts;
        private System.Windows.Forms.FlowLayoutPanel panelCategories;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.ListView listCart;
        private System.Windows.Forms.ColumnHeader columnProducto;
        private System.Windows.Forms.ColumnHeader columnCant;
        private System.Windows.Forms.ColumnHeader columnPrecio;
        private System.Windows.Forms.ColumnHeader columnTotal;
        private System.Windows.Forms.Panel panelTotals;
        private System.Windows.Forms.Button btnClearCart;
        private System.Windows.Forms.Button btnCard;
        private System.Windows.Forms.Button btnCash;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Panel panelCustomer;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblCartTitle;
    }
}
