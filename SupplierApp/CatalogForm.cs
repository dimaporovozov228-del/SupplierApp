using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class CatalogForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount currentUser;
        private readonly ShoppingCart cart = new ShoppingCart();
        private List<Product> products = new List<Product>();
        private string lastAddress = string.Empty;
        public CatalogForm() { InitializeComponent(); Theme.Apply(this); UpdateCart(); }
        public CatalogForm(AppDatabase database, UserAccount user) : this()
        {
            this.database = database;
            currentUser = user;
            lblUser.Text = "Здравствуйте, " + user.FullName + "!  ·  " + user.UserName;
        }

        private void CatalogForm_Shown(object sender, EventArgs e) { LoadProducts(); }
        private void LoadProducts()
        {
            if (database == null) return;
            try { products = database.GetProducts(); ApplyFilter(); }
            catch (Exception exception)
            {
                products.Clear(); ApplyFilter();
                lblCount.Text = "Не удалось загрузить каталог. Нажмите «Обновить».";
                UiErrors.Show(this, exception);
            }
        }

        private void ApplyFilter()
        {
            string query = txtSearch.Text.Trim();
            IEnumerable<Product> filtered = products.Where(p => p.ProductName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0);
            switch (cmbSort.SelectedIndex)
            {
                case 1: filtered = filtered.OrderBy(p => p.UnitPrice); break;
                case 2: filtered = filtered.OrderByDescending(p => p.UnitPrice); break;
                default: filtered = filtered.OrderBy(p => p.ProductName); break;
            }
            var visible = filtered.ToList();
            gridProducts.DataSource = visible;
            lblCount.Text = visible.Count == 0 ? "Товары не найдены. Измените запрос или обновите каталог." : "Найдено товаров: " + visible.Count + "  ·  Цены за единицу товара";
            btnOrder.Enabled = visible.Count > 0;
        }

        private void UpdateCart()
        {
            btnCart.Text = "Корзина (" + cart.Items.Count + ")  ·  " + Theme.Money(cart.TotalAmount);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            var product = gridProducts.CurrentRow == null ? null : gridProducts.CurrentRow.DataBoundItem as Product;
            if (product == null) return;
            try
            {
                cart.Add(product, numQuantity.Value);
                UpdateCart();
                lblNotice.Text = "Добавлено: " + product.ProductName + ". Перейдите в корзину для оформления.";
            }
            catch (Exception exception) { UiErrors.Show(this, exception); }
        }

        private void OpenCart(object sender, EventArgs e)
        {
            if (database == null || currentUser == null) return;
            using (var form = new CartForm(database, currentUser, cart, lastAddress))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    lastAddress = form.DeliveryAddress;
                    lblNotice.Text = "Заказы оформлены. Посмотреть их можно в разделе «Мои заказы».";
                }
            }
            UpdateCart();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            if (database == null || currentUser == null) return;
            using (var form = new OrdersForm(database, currentUser)) form.ShowDialog(this);
        }
        private void btnRefresh_Click(object sender, EventArgs e) { LoadProducts(); }
        private void btnExit_Click(object sender, EventArgs e) { Close(); }
        private void ClosingCatalog(object sender, FormClosingEventArgs e)
        {
            if (cart.Items.Count > 0 && MessageBox.Show(this, "В корзине остались товары. Выйти и очистить корзину?",
                "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
        }
    }
}
