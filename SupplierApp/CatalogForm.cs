using System;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class CatalogForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount currentUser;
        public CatalogForm() { InitializeComponent(); }
        public CatalogForm(AppDatabase database, UserAccount user) : this()
        {
            this.database = database;
            currentUser = user;
            lblUser.Text = "Пользователь: " + user.FullName + " (" + user.UserName + ")";
        }

        private void CatalogForm_Shown(object sender, EventArgs e) { LoadProducts(); }

        private void LoadProducts()
        {
            if (database == null) return;
            try
            {
                lstProducts.DataSource = database.GetProducts();
                lstProducts.DisplayMember = "DisplayName";
                lstProducts.ValueMember = "ProductId";
                btnOrder.Enabled = lstProducts.Items.Count > 0;
                if (lstProducts.Items.Count > 0) lstProducts.SelectedIndex = 0;
                else MessageBox.Show(this, "В базе нет доступных товаров.", "Каталог");
            }
            catch (Exception exception)
            {
                lstProducts.DataSource = null;
                btnOrder.Enabled = false;
                UiErrors.Show(this, exception);
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            var product = lstProducts.SelectedItem as Product;
            if (product == null || database == null || currentUser == null) return;
            using (var form = new OrderForm(database, currentUser, product)) form.ShowDialog(this);
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            if (database == null || currentUser == null) return;
            using (var form = new OrdersForm(database, currentUser)) form.ShowDialog(this);
        }

        private void btnRefresh_Click(object sender, EventArgs e) { LoadProducts(); }
        private void btnExit_Click(object sender, EventArgs e) { Close(); }
    }
}
