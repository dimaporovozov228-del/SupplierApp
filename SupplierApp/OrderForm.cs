using System;
using System.Globalization;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class OrderForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount currentUser;
        private readonly Product product;
        public OrderForm()
        {
            InitializeComponent();
            Theme.AddHeader(this, lblTitle, "Оформление заказа", "Проверьте количество и укажите адрес доставки.");
            btnBack.Tag = "secondary";
            Theme.Apply(this);
        }
        public OrderForm(AppDatabase database, UserAccount user, Product product) : this()
        {
            this.database = database;
            currentUser = user;
            this.product = product;
            txtProduct.Text = product.ProductName;
            lblQuantity.Text = "Количество, " + product.Unit;
            lblPrice.Text = string.Format(CultureInfo.GetCultureInfo("ru-RU"),
                "Цена: {0:N2} руб. / {1}", product.UnitPrice, product.Unit);
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            if (product != null)
                lblTotal.Text = string.Format(CultureInfo.GetCultureInfo("ru-RU"),
                    "Итого: {0:N2} руб.", decimal.Round(product.UnitPrice * numQuantity.Value, 2, MidpointRounding.ToEven));
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e) { UpdateTotal(); }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (database == null || currentUser == null || product == null) return;
            btnSend.Enabled = false;
            UseWaitCursor = true;
            try
            {
                int orderId = database.CreateOrder(currentUser, product, numQuantity.Value, txtAddress.Text);
                MessageBox.Show(this, "Заказ № " + orderId + " сохранен в базе данных.\n" + lblTotal.Text +
                    "\nПроверить запись можно в окне «Мои заказы».", "Заказ оформлен",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception) { UiErrors.Show(this, exception); }
            finally { UseWaitCursor = false; if (!IsDisposed) btnSend.Enabled = true; }
        }

        private void btnBack_Click(object sender, EventArgs e) { Close(); }
    }
}
