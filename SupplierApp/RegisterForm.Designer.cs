namespace SupplierApp
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblName, lblLogin, lblPassword, lblRepeat, lblHint;
        private System.Windows.Forms.TextBox txtName, txtLogin, txtPassword, txtRepeat;


        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblName = new System.Windows.Forms.Label();
            lblLogin = new System.Windows.Forms.Label();
            lblPassword = new System.Windows.Forms.Label();
            lblRepeat = new System.Windows.Forms.Label();
            lblHint = new System.Windows.Forms.Label();
            txtName = new System.Windows.Forms.TextBox();
            txtLogin = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            txtRepeat = new System.Windows.Forms.TextBox();
            SuspendLayout();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular);
            lblTitle.Location = new System.Drawing.Point(28, 20);
            lblTitle.Text = "Регистрация пользователя";
            lblName.AutoSize = true;
            lblName.Location = new System.Drawing.Point(30, 82);
            lblName.Text = "Имя";
            txtName.Name = "txtName";
            txtName.Location = new System.Drawing.Point(170, 78);
            txtName.Size = new System.Drawing.Size(300, 23);
            txtName.MaxLength = 100;
            txtName.TabIndex = 0;
            lblLogin.AutoSize = true;
            lblLogin.Location = new System.Drawing.Point(30, 126);
            lblLogin.Text = "Логин";
            txtLogin.Name = "txtLogin";
            txtLogin.Location = new System.Drawing.Point(170, 122);
            txtLogin.Size = new System.Drawing.Size(300, 23);
            txtLogin.MaxLength = 50;
            txtLogin.TabIndex = 1;
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(30, 170);
            lblPassword.Text = "Пароль";
            txtPassword.Name = "txtPassword";
            txtPassword.Location = new System.Drawing.Point(170, 166);
            txtPassword.Size = new System.Drawing.Size(300, 23);
            txtPassword.MaxLength = 128;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 2;
            lblRepeat.AutoSize = true;
            lblRepeat.Location = new System.Drawing.Point(30, 214);
            lblRepeat.Text = "Повтор пароля";
            txtRepeat.Name = "txtRepeat";
            txtRepeat.Location = new System.Drawing.Point(170, 210);
            txtRepeat.Size = new System.Drawing.Size(300, 23);
            txtRepeat.MaxLength = 128;
            txtRepeat.UseSystemPasswordChar = true;
            txtRepeat.TabIndex = 3;
            lblHint.Location = new System.Drawing.Point(30, 255);
            lblHint.Size = new System.Drawing.Size(440, 55);
            lblHint.Text = "Логин: 3–50 латинских букв, цифр или символов _ . -\r\nПароль: 8–128 символов. Имя: 2–100 символов.";
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            ClientSize = new System.Drawing.Size(505, 396);
            Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblName, txtName, lblLogin, txtLogin, lblPassword, txtPassword, lblRepeat, txtRepeat, lblHint, });
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "SupplierApp — регистрация";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
