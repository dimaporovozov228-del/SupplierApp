using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtLogin, txtPassword;
        private Button btnLogin, btnRegister, btnExit;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            SuspendLayout();
            Theme.Setup(this, "добро пожаловать", 660, 560);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(Theme.Header("Хорошие продукты. Простой заказ.", "Войдите в аккаунт, чтобы собрать корзину и следить за покупками.", 660));
            Controls.Add(Theme.Label("Вход в личный кабинет", 40, 133, 580, 40, 18F));
            Controls.Add(Theme.Label("Логин", 40, 189, 580, 26));
            txtLogin = new TextBox { Name = "txtLogin", AccessibleName = "Логин", Location = new Point(40, 222), Size = new Size(580, 28), MaxLength = 50, TabIndex = 0, Font = new Font("Segoe UI", 11F) };
            Controls.Add(Theme.Label("Пароль", 40, 267, 400, 26));
            txtPassword = new TextBox { Name = "txtPassword", AccessibleName = "Пароль", Location = new Point(40, 300), Size = new Size(580, 28), MaxLength = 128, UseSystemPasswordChar = true, TabIndex = 1, Font = new Font("Segoe UI", 11F) };
            var show = new CheckBox { Text = "Показать пароль", Location = new Point(40, 344), Size = new Size(220, 25), TabIndex = 2 };
            show.CheckedChanged += (s, e) => txtPassword.UseSystemPasswordChar = !show.Checked;
            btnLogin = Theme.Button("Войти", 40, 390, 180, btnLogin_Click);
            btnLogin.TabIndex = 3;
            btnRegister = Theme.Button("Регистрация", 240, 390, 180, btnRegister_Click, true);
            btnRegister.TabIndex = 4;
            btnExit = Theme.Button("Выход", 440, 390, 180, btnExit_Click, true);
            btnExit.TabIndex = 5;
            var demo = Theme.Label("Учебный вход: student / Student123!\r\nSUPPLIERAPP 4.0 · Ваш любимый поставщик", 40, 460, 580, 62);
            demo.ForeColor = Theme.Muted;
            Controls.AddRange(new Control[] { txtLogin, txtPassword, show, btnLogin, btnRegister, btnExit, demo });
            AcceptButton = btnLogin;
            CancelButton = btnExit;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
