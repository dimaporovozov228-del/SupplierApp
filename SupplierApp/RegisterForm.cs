using System;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class RegisterForm : Form
    {
        private readonly AppDatabase database;
        public string RegisteredUserName { get; private set; }
        public RegisterForm() { InitializeComponent(); Theme.Apply(this); }
        public RegisterForm(AppDatabase database) : this() { this.database = database; }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (database == null) return;
            if (txtPassword.Text != txtRepeat.Text)
            {
                MessageBox.Show(this, "Пароли не совпадают.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnCreate.Enabled = false;
            UseWaitCursor = true;
            try
            {
                var account = database.Register(txtLogin.Text, txtName.Text, txtPassword.Text);
                RegisteredUserName = account.UserName;
                txtPassword.Clear();
                txtRepeat.Clear();
                MessageBox.Show(this, "Учетная запись создана. Теперь войдите с выбранным паролем.", "Регистрация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception) { UiErrors.Show(this, exception); }
            finally { UseWaitCursor = false; if (!IsDisposed) btnCreate.Enabled = true; }
        }

        private void btnCancel_Click(object sender, EventArgs e) { Close(); }
    }
}
