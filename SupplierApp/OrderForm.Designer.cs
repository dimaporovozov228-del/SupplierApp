namespace SupplierApp
{
    partial class OrderForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblProduct, lblQuantity, lblAddress, lblPrice, lblTotal;
        private System.Windows.Forms.TextBox txtProduct, txtAddress;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnSend, btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblProduct = new System.Windows.Forms.Label();
            lblQuantity = new System.Windows.Forms.Label();
            lblAddress = new System.Windows.Forms.Label();
            lblPrice = new System.Windows.Forms.Label();
            lblTotal = new System.Windows.Forms.Label();
            txtProduct = new System.Windows.Forms.TextBox();
            txtAddress = new System.Windows.Forms.TextBox();
            numQuantity = new System.Windows.Forms.NumericUpDown();
            btnSend = new System.Windows.Forms.Button();
            btnBack = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numQuantity).BeginInit();
            SuspendLayout();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            lblTitle.Location = new System.Drawing.Point(28, 20);
            lblTitle.Text = "Оформление заказа";
            lblProduct.AutoSize = true;
            lblProduct.Location = new System.Drawing.Point(30, 79);
            lblProduct.Text = "Товар";
            txtProduct.Name = "txtProduct";
            txtProduct.Location = new System.Drawing.Point(160, 76);
            txtProduct.Size = new System.Drawing.Size(335, 23);
            txtProduct.ReadOnly = true;
            txtProduct.TabStop = false;
            lblPrice.Location = new System.Drawing.Point(160, 112);
            lblPrice.Size = new System.Drawing.Size(335, 25);
            lblPrice.Text = "Цена";
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new System.Drawing.Point(30, 157);
            lblQuantity.Text = "Количество, кг";
            numQuantity.Name = "numQuantity";
            numQuantity.Location = new System.Drawing.Point(160, 154);
            numQuantity.Size = new System.Drawing.Size(160, 23);
            numQuantity.DecimalPlaces = 3;
            numQuantity.Minimum = 0.001M;
            numQuantity.Maximum = 10000M;
            numQuantity.Value = 1M;
            numQuantity.Increment = 0.5M;
            numQuantity.TabIndex = 0;
            numQuantity.ValueChanged += new System.EventHandler(numQuantity_ValueChanged);
            lblAddress.AutoSize = true;
            lblAddress.Location = new System.Drawing.Point(30, 204);
            lblAddress.Text = "Адрес доставки";
            txtAddress.Name = "txtAddress";
            txtAddress.Location = new System.Drawing.Point(160, 200);
            txtAddress.Size = new System.Drawing.Size(335, 65);
            txtAddress.Multiline = true;
            txtAddress.MaxLength = 255;
            txtAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtAddress.TabIndex = 1;
            lblTotal.Location = new System.Drawing.Point(160, 284);
            lblTotal.Size = new System.Drawing.Size(335, 32);
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            lblTotal.Text = "Итого";
            btnSend.Location = new System.Drawing.Point(160, 339);
            btnSend.Size = new System.Drawing.Size(155, 36);
            btnSend.Text = "Сохранить заказ";
            btnSend.TabIndex = 2;
            btnSend.Click += new System.EventHandler(btnSend_Click);
            btnBack.Location = new System.Drawing.Point(340, 339);
            btnBack.Size = new System.Drawing.Size(155, 36);
            btnBack.Text = "Назад";
            btnBack.TabIndex = 3;
            btnBack.Click += new System.EventHandler(btnBack_Click);
            AcceptButton = btnSend;
            CancelButton = btnBack;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            ClientSize = new System.Drawing.Size(530, 405);
            Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblProduct, txtProduct, lblPrice, lblQuantity, numQuantity, lblAddress, txtAddress, lblTotal, btnSend, btnBack });
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SupplierApp — новый заказ";
            ((System.ComponentModel.ISupportInitialize)numQuantity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
