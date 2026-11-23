namespace SupplierApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle, lblLogin, lblPassword, lblDemo;
        private System.Windows.Forms.TextBox txtLogin, txtPassword;
        private System.Windows.Forms.Button btnLogin, btnRegister, btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new System.Windows.Forms.Label();
            lblLogin = new System.Windows.Forms.Label();
            lblPassword = new System.Windows.Forms.Label();
            lblDemo = new System.Windows.Forms.Label();
            txtLogin = new System.Windows.Forms.TextBox();
            txtPassword = new System.Windows.Forms.TextBox();
            btnLogin = new System.Windows.Forms.Button();
            btnRegister = new System.Windows.Forms.Button();
            btnExit = new System.Windows.Forms.Button();
            SuspendLayout();
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular);
            lblTitle.Location = new System.Drawing.Point(38, 25);
            lblTitle.Text = "Ваш любимый поставщик";
            lblLogin.AutoSize = true;
            lblLogin.Location = new System.Drawing.Point(40, 92);
            lblLogin.Text = "Логин";
            txtLogin.Name = "txtLogin";
            txtLogin.Location = new System.Drawing.Point(140, 88);
            txtLogin.Size = new System.Drawing.Size(290, 23);
            txtLogin.MaxLength = 50;
            txtLogin.TabIndex = 0;
            lblPassword.AutoSize = true;
            lblPassword.Location = new System.Drawing.Point(40, 134);
            lblPassword.Text = "Пароль";
            txtPassword.Name = "txtPassword";
            txtPassword.Location = new System.Drawing.Point(140, 130);
            txtPassword.Size = new System.Drawing.Size(290, 23);
            txtPassword.MaxLength = 128;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TabIndex = 1;
            btnLogin.Location = new System.Drawing.Point(40, 186);
            btnLogin.Size = new System.Drawing.Size(115, 35);
            btnLogin.Text = "Войти";
            btnLogin.TabIndex = 2;
            btnLogin.Click += new System.EventHandler(btnLogin_Click);
            btnRegister.Location = new System.Drawing.Point(170, 186);
            btnRegister.Size = new System.Drawing.Size(130, 35);
            btnRegister.Text = "Регистрация";
            btnRegister.TabIndex = 3;
            btnRegister.Click += new System.EventHandler(btnRegister_Click);
            btnExit.Location = new System.Drawing.Point(315, 186);
            btnExit.Size = new System.Drawing.Size(115, 35);
            btnExit.Text = "Выход";
            btnExit.TabIndex = 4;
            btnExit.Click += new System.EventHandler(btnExit_Click);
            lblDemo.Location = new System.Drawing.Point(40, 244);
            lblDemo.Size = new System.Drawing.Size(400, 50);
            lblDemo.Text = "Учебная учетная запись\r\nЛогин: student    Пароль: Student123!";
            AcceptButton = btnLogin;
            CancelButton = btnExit;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Font = new System.Drawing.Font("Segoe UI", 9F);
            ClientSize = new System.Drawing.Size(475, 310);
            Controls.AddRange(new System.Windows.Forms.Control[] { lblTitle, lblLogin, txtLogin, lblPassword, txtPassword, btnLogin, btnRegister, btnExit, lblDemo });
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SupplierApp — вход";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
