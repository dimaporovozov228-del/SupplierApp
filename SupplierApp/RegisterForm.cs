using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class RegisterForm : Form
    {
        private readonly AppDatabase database;
        public string RegisteredUserName { get; private set; }
        public RegisterForm()
        {
            InitializeComponent();
            Theme.AddHeader(this, lblTitle, "Создайте аккаунт", "Сохраняйте заказы и управляйте покупками.");
            btnCancel.Tag = "secondary";
            Theme.Apply(this);
        }
        public RegisterForm(AppDatabase database) : this() { this.database = database; }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (database == null) return;
            if (txtPassword.Text != txtRepeat.Text)
            {
                MessageBox.Show(this, "Пароли не совпадают.", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnCreate.Enabled = false;
            txtLogin.Enabled = txtName.Enabled = txtPassword.Enabled = txtRepeat.Enabled = false;
            string login = txtLogin.Text, fullName = txtName.Text, password = txtPassword.Text;
            UseWaitCursor = true;
            try
            {
                var account = await Task.Run(() => database.Register(login, fullName, password));
                if (IsDisposed) return;
                RegisteredUserName = account.UserName;
                txtPassword.Clear();
                txtRepeat.Clear();
                MessageBox.Show(this, "Учетная запись создана. Теперь войдите с выбранным паролем.", "Регистрация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception) { if (!IsDisposed) UiErrors.Show(this, exception); }
            finally
            {
                if (!IsDisposed)
                {
                    UseWaitCursor = false;
                    btnCreate.Enabled = true;
                    txtLogin.Enabled = txtName.Enabled = txtPassword.Enabled = txtRepeat.Enabled = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) { Close(); }
    }
}
