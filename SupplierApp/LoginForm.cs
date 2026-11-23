using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class LoginForm : Form
    {
        private readonly AppDatabase database;
        public LoginForm() { InitializeComponent(); Theme.Apply(this); }
        public LoginForm(AppDatabase database) : this() { this.database = database; }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (database == null) return;
            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Введите логин и пароль.", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            UserAccount account;
            string login = txtLogin.Text, password = txtPassword.Text;
            SetBusy(true);
            try { account = await Task.Run(() => database.Authenticate(login, password)); }
            catch (Exception exception) { if (!IsDisposed) UiErrors.Show(this, exception); return; }
            finally { if (!IsDisposed) SetBusy(false); }
            if (IsDisposed) return;
            if (account == null)
            {
                MessageBox.Show(this, "Неверный логин или пароль.", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }
            txtPassword.Clear();
            Hide();
            try
            {
                using (var catalog = new CatalogForm(database, account)) catalog.ShowDialog();
            }
            finally { Close(); }
        }

        private void SetBusy(bool value)
        {
            UseWaitCursor = value;
            btnLogin.Enabled = !value;
            btnRegister.Enabled = !value;
            txtLogin.Enabled = txtPassword.Enabled = !value;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (database == null) return;
            using (var form = new RegisterForm(database))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    txtLogin.Text = form.RegisteredUserName;
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e) { Close(); }
    }
}
