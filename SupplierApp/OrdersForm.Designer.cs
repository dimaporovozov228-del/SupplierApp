namespace SupplierApp
{
    partial class OrdersForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblCount, lblDatabase;
        private System.Windows.Forms.TextBox txtDatabasePath;
        private System.Windows.Forms.DataGridView gridOrders;


        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblCount = new System.Windows.Forms.Label();
            lblDatabase = new System.Windows.Forms.Label();
            txtDatabasePath = new System.Windows.Forms.TextBox();
            gridOrders = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)gridOrders).BeginInit();
            SuspendLayout();
            lblTitle.Location = new System.Drawing.Point(22, 20);
            lblTitle.Size = new System.Drawing.Size(940, 35);
            lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            lblTitle.AutoEllipsis = true;
            lblTitle.Text = "Мои заказы";
            gridOrders.Name = "gridOrders";
            gridOrders.Location = new System.Drawing.Point(25, 68);
            gridOrders.Size = new System.Drawing.Size(950, 330);
            gridOrders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gridOrders.ReadOnly = true;
            gridOrders.AllowUserToAddRows = false;
            gridOrders.AllowUserToDeleteRows = false;
            gridOrders.MultiSelect = false;
            gridOrders.RowHeadersVisible = false;
            gridOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gridOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridOrders.BackgroundColor = System.Drawing.SystemColors.Window;
            gridOrders.TabIndex = 0;
            lblCount.Location = new System.Drawing.Point(25, 413);
            lblCount.Size = new System.Drawing.Size(600, 25);
            lblCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblCount.Text = "Заказы";
            lblDatabase.Location = new System.Drawing.Point(25, 453);
            lblDatabase.Size = new System.Drawing.Size(95, 23);
            lblDatabase.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblDatabase.Text = "Файл базы";
            txtDatabasePath.Location = new System.Drawing.Point(125, 450);
            txtDatabasePath.Size = new System.Drawing.Size(850, 23);
            txtDatabasePath.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtDatabasePath.ReadOnly = true;
            txtDatabasePath.TabIndex = 3;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            ClientSize = new System.Drawing.Size(1000, 552);
            MinimumSize = new System.Drawing.Size(900, 500);
            Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, gridOrders, lblCount, lblDatabase, txtDatabasePath, });
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SupplierApp — история заказов";
            ((System.ComponentModel.ISupportInitialize)gridOrders).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
