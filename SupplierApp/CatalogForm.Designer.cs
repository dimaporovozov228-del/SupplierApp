namespace SupplierApp
{
    partial class CatalogForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblUser, lblHint;
        private System.Windows.Forms.ListBox lstProducts;
        private System.Windows.Forms.Button btnOrder, btnOrders, btnRefresh, btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblUser = new System.Windows.Forms.Label();
            lblHint = new System.Windows.Forms.Label();
            lstProducts = new System.Windows.Forms.ListBox();
            btnOrder = new System.Windows.Forms.Button();
            btnOrders = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnExit = new System.Windows.Forms.Button();
            SuspendLayout();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            lblTitle.Location = new System.Drawing.Point(28, 20);
            lblTitle.Text = "Каталог товаров";
            lblUser.Location = new System.Drawing.Point(30, 62);
            lblUser.Size = new System.Drawing.Size(550, 25);
            lblUser.AutoEllipsis = true;
            lblUser.Text = "Пользователь";
            lstProducts.Name = "lstProducts";
            lstProducts.Location = new System.Drawing.Point(30, 100);
            lstProducts.Size = new System.Drawing.Size(550, 165);
            lstProducts.ItemHeight = 22;
            lstProducts.Font = new System.Drawing.Font("Segoe UI", 11F);
            lstProducts.HorizontalScrollbar = true;
            lstProducts.TabIndex = 0;
            lblHint.Location = new System.Drawing.Point(30, 278);
            lblHint.Size = new System.Drawing.Size(550, 35);
            lblHint.Text = "Цены учебные. Заказы сохраняются в локальной базе Access.";
            btnOrder.Location = new System.Drawing.Point(30, 328);
            btnOrder.Size = new System.Drawing.Size(130, 36);
            btnOrder.Text = "Оформить";
            btnOrder.TabIndex = 1;
            btnOrder.Click += new System.EventHandler(btnOrder_Click);
            btnOrders.Location = new System.Drawing.Point(170, 328);
            btnOrders.Size = new System.Drawing.Size(130, 36);
            btnOrders.Text = "Мои заказы";
            btnOrders.TabIndex = 2;
            btnOrders.Click += new System.EventHandler(btnOrders_Click);
            btnRefresh.Location = new System.Drawing.Point(310, 328);
            btnRefresh.Size = new System.Drawing.Size(130, 36);
            btnRefresh.Text = "Обновить";
            btnRefresh.TabIndex = 3;
            btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
            btnExit.Location = new System.Drawing.Point(450, 328);
            btnExit.Size = new System.Drawing.Size(130, 36);
            btnExit.Text = "Выход";
            btnExit.TabIndex = 4;
            btnExit.Click += new System.EventHandler(btnExit_Click);
            AcceptButton = btnOrder;
            CancelButton = btnExit;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            ClientSize = new System.Drawing.Size(610, 395);
            Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblUser, lstProducts, lblHint, btnOrder, btnOrders, btnRefresh, btnExit });
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SupplierApp — каталог";
            Shown += new System.EventHandler(CatalogForm_Shown);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
