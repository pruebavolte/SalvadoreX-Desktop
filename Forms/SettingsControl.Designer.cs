namespace SalvadoreXDesktop.Forms
{
    partial class SettingsControl
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBusinessSection = new System.Windows.Forms.Label();
            this.lblBusinessName = new System.Windows.Forms.Label();
            this.txtBusinessName = new System.Windows.Forms.TextBox();
            this.lblBusinessAddress = new System.Windows.Forms.Label();
            this.txtBusinessAddress = new System.Windows.Forms.TextBox();
            this.lblBusinessPhone = new System.Windows.Forms.Label();
            this.txtBusinessPhone = new System.Windows.Forms.TextBox();
            this.lblBusinessRfc = new System.Windows.Forms.Label();
            this.txtBusinessRfc = new System.Windows.Forms.TextBox();
            this.lblTaxRate = new System.Windows.Forms.Label();
            this.numTaxRate = new System.Windows.Forms.NumericUpDown();
            this.lblSyncSection = new System.Windows.Forms.Label();
            this.lblSyncStatus = new System.Windows.Forms.Label();
            this.lblSupabaseUrl = new System.Windows.Forms.Label();
            this.txtSupabaseUrl = new System.Windows.Forms.TextBox();
            this.lblSupabaseKey = new System.Windows.Forms.Label();
            this.txtSupabaseKey = new System.Windows.Forms.TextBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnSyncNow = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numTaxRate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Configuracion";
            // 
            // lblBusinessSection
            // 
            this.lblBusinessSection.AutoSize = true;
            this.lblBusinessSection.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBusinessSection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblBusinessSection.Location = new System.Drawing.Point(20, 60);
            this.lblBusinessSection.Name = "lblBusinessSection";
            this.lblBusinessSection.Size = new System.Drawing.Size(196, 32);
            this.lblBusinessSection.TabIndex = 1;
            this.lblBusinessSection.Text = "Datos del Negocio";
            // 
            // lblBusinessName
            // 
            this.lblBusinessName.AutoSize = true;
            this.lblBusinessName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusinessName.Location = new System.Drawing.Point(20, 105);
            this.lblBusinessName.Name = "lblBusinessName";
            this.lblBusinessName.Size = new System.Drawing.Size(166, 23);
            this.lblBusinessName.TabIndex = 2;
            this.lblBusinessName.Text = "Nombre del Negocio:";
            // 
            // txtBusinessName
            // 
            this.txtBusinessName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusinessName.Location = new System.Drawing.Point(200, 100);
            this.txtBusinessName.Name = "txtBusinessName";
            this.txtBusinessName.Size = new System.Drawing.Size(350, 30);
            this.txtBusinessName.TabIndex = 3;
            // 
            // lblBusinessAddress
            // 
            this.lblBusinessAddress.AutoSize = true;
            this.lblBusinessAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusinessAddress.Location = new System.Drawing.Point(20, 145);
            this.lblBusinessAddress.Name = "lblBusinessAddress";
            this.lblBusinessAddress.Size = new System.Drawing.Size(80, 23);
            this.lblBusinessAddress.TabIndex = 4;
            this.lblBusinessAddress.Text = "Direccion:";
            // 
            // txtBusinessAddress
            // 
            this.txtBusinessAddress.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusinessAddress.Location = new System.Drawing.Point(200, 140);
            this.txtBusinessAddress.Name = "txtBusinessAddress";
            this.txtBusinessAddress.Size = new System.Drawing.Size(350, 30);
            this.txtBusinessAddress.TabIndex = 5;
            // 
            // lblBusinessPhone
            // 
            this.lblBusinessPhone.AutoSize = true;
            this.lblBusinessPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusinessPhone.Location = new System.Drawing.Point(20, 185);
            this.lblBusinessPhone.Name = "lblBusinessPhone";
            this.lblBusinessPhone.Size = new System.Drawing.Size(74, 23);
            this.lblBusinessPhone.TabIndex = 6;
            this.lblBusinessPhone.Text = "Telefono:";
            // 
            // txtBusinessPhone
            // 
            this.txtBusinessPhone.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusinessPhone.Location = new System.Drawing.Point(200, 180);
            this.txtBusinessPhone.Name = "txtBusinessPhone";
            this.txtBusinessPhone.Size = new System.Drawing.Size(350, 30);
            this.txtBusinessPhone.TabIndex = 7;
            // 
            // lblBusinessRfc
            // 
            this.lblBusinessRfc.AutoSize = true;
            this.lblBusinessRfc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBusinessRfc.Location = new System.Drawing.Point(20, 225);
            this.lblBusinessRfc.Name = "lblBusinessRfc";
            this.lblBusinessRfc.Size = new System.Drawing.Size(40, 23);
            this.lblBusinessRfc.TabIndex = 8;
            this.lblBusinessRfc.Text = "RFC:";
            // 
            // txtBusinessRfc
            // 
            this.txtBusinessRfc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBusinessRfc.Location = new System.Drawing.Point(200, 220);
            this.txtBusinessRfc.Name = "txtBusinessRfc";
            this.txtBusinessRfc.Size = new System.Drawing.Size(350, 30);
            this.txtBusinessRfc.TabIndex = 9;
            // 
            // lblTaxRate
            // 
            this.lblTaxRate.AutoSize = true;
            this.lblTaxRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTaxRate.Location = new System.Drawing.Point(20, 265);
            this.lblTaxRate.Name = "lblTaxRate";
            this.lblTaxRate.Size = new System.Drawing.Size(119, 23);
            this.lblTaxRate.TabIndex = 10;
            this.lblTaxRate.Text = "Tasa de IVA (%):";
            // 
            // numTaxRate
            // 
            this.numTaxRate.DecimalPlaces = 2;
            this.numTaxRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numTaxRate.Location = new System.Drawing.Point(200, 260);
            this.numTaxRate.Name = "numTaxRate";
            this.numTaxRate.Size = new System.Drawing.Size(100, 30);
            this.numTaxRate.TabIndex = 11;
            this.numTaxRate.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // lblSyncSection
            // 
            this.lblSyncSection.AutoSize = true;
            this.lblSyncSection.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSyncSection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.lblSyncSection.Location = new System.Drawing.Point(20, 310);
            this.lblSyncSection.Name = "lblSyncSection";
            this.lblSyncSection.Size = new System.Drawing.Size(287, 32);
            this.lblSyncSection.TabIndex = 12;
            this.lblSyncSection.Text = "Sincronizacion en la Nube";
            // 
            // lblSyncStatus
            // 
            this.lblSyncStatus.AutoSize = true;
            this.lblSyncStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSyncStatus.Location = new System.Drawing.Point(20, 350);
            this.lblSyncStatus.Name = "lblSyncStatus";
            this.lblSyncStatus.Size = new System.Drawing.Size(165, 25);
            this.lblSyncStatus.TabIndex = 13;
            this.lblSyncStatus.Text = "Estado: Verificando...";
            // 
            // lblSupabaseUrl
            // 
            this.lblSupabaseUrl.AutoSize = true;
            this.lblSupabaseUrl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSupabaseUrl.Location = new System.Drawing.Point(20, 390);
            this.lblSupabaseUrl.Name = "lblSupabaseUrl";
            this.lblSupabaseUrl.Size = new System.Drawing.Size(112, 23);
            this.lblSupabaseUrl.TabIndex = 14;
            this.lblSupabaseUrl.Text = "URL Supabase:";
            // 
            // txtSupabaseUrl
            // 
            this.txtSupabaseUrl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSupabaseUrl.Location = new System.Drawing.Point(200, 385);
            this.txtSupabaseUrl.Name = "txtSupabaseUrl";
            this.txtSupabaseUrl.ReadOnly = true;
            this.txtSupabaseUrl.Size = new System.Drawing.Size(350, 30);
            this.txtSupabaseUrl.TabIndex = 15;
            // 
            // lblSupabaseKey
            // 
            this.lblSupabaseKey.AutoSize = true;
            this.lblSupabaseKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSupabaseKey.Location = new System.Drawing.Point(20, 430);
            this.lblSupabaseKey.Name = "lblSupabaseKey";
            this.lblSupabaseKey.Size = new System.Drawing.Size(65, 23);
            this.lblSupabaseKey.TabIndex = 16;
            this.lblSupabaseKey.Text = "API Key:";
            // 
            // txtSupabaseKey
            // 
            this.txtSupabaseKey.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSupabaseKey.Location = new System.Drawing.Point(200, 425);
            this.txtSupabaseKey.Name = "txtSupabaseKey";
            this.txtSupabaseKey.PasswordChar = '*';
            this.txtSupabaseKey.ReadOnly = true;
            this.txtSupabaseKey.Size = new System.Drawing.Size(350, 30);
            this.txtSupabaseKey.TabIndex = 17;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.btnTestConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestConnection.FlatAppearance.BorderSize = 0;
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.Location = new System.Drawing.Point(200, 475);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(140, 40);
            this.btnTestConnection.TabIndex = 18;
            this.btnTestConnection.Text = "Probar Conexion";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.BtnTestConnection_Click);
            // 
            // btnSyncNow
            // 
            this.btnSyncNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnSyncNow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncNow.FlatAppearance.BorderSize = 0;
            this.btnSyncNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncNow.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSyncNow.ForeColor = System.Drawing.Color.White;
            this.btnSyncNow.Location = new System.Drawing.Point(350, 475);
            this.btnSyncNow.Name = "btnSyncNow";
            this.btnSyncNow.Size = new System.Drawing.Size(140, 40);
            this.btnSyncNow.TabIndex = 19;
            this.btnSyncNow.Text = "Sincronizar Ahora";
            this.btnSyncNow.UseVisualStyleBackColor = false;
            this.btnSyncNow.Click += new System.EventHandler(this.BtnSyncNow_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(200, 535);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 45);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Guardar Configuracion";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSyncNow);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.txtSupabaseKey);
            this.Controls.Add(this.lblSupabaseKey);
            this.Controls.Add(this.txtSupabaseUrl);
            this.Controls.Add(this.lblSupabaseUrl);
            this.Controls.Add(this.lblSyncStatus);
            this.Controls.Add(this.lblSyncSection);
            this.Controls.Add(this.numTaxRate);
            this.Controls.Add(this.lblTaxRate);
            this.Controls.Add(this.txtBusinessRfc);
            this.Controls.Add(this.lblBusinessRfc);
            this.Controls.Add(this.txtBusinessPhone);
            this.Controls.Add(this.lblBusinessPhone);
            this.Controls.Add(this.txtBusinessAddress);
            this.Controls.Add(this.lblBusinessAddress);
            this.Controls.Add(this.txtBusinessName);
            this.Controls.Add(this.lblBusinessName);
            this.Controls.Add(this.lblBusinessSection);
            this.Controls.Add(this.lblTitle);
            this.Name = "SettingsControl";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(1000, 600);
            this.Load += new System.EventHandler(this.SettingsControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTaxRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBusinessSection;
        private System.Windows.Forms.Label lblBusinessName;
        private System.Windows.Forms.TextBox txtBusinessName;
        private System.Windows.Forms.Label lblBusinessAddress;
        private System.Windows.Forms.TextBox txtBusinessAddress;
        private System.Windows.Forms.Label lblBusinessPhone;
        private System.Windows.Forms.TextBox txtBusinessPhone;
        private System.Windows.Forms.Label lblBusinessRfc;
        private System.Windows.Forms.TextBox txtBusinessRfc;
        private System.Windows.Forms.Label lblTaxRate;
        private System.Windows.Forms.NumericUpDown numTaxRate;
        private System.Windows.Forms.Label lblSyncSection;
        private System.Windows.Forms.Label lblSyncStatus;
        private System.Windows.Forms.Label lblSupabaseUrl;
        private System.Windows.Forms.TextBox txtSupabaseUrl;
        private System.Windows.Forms.Label lblSupabaseKey;
        private System.Windows.Forms.TextBox txtSupabaseKey;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnSyncNow;
        private System.Windows.Forms.Button btnSave;
    }
}
