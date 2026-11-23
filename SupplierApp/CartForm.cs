using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SupplierApp
{
    public sealed class CartForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount user;
        private readonly ShoppingCart cart;
        private readonly DataGridView grid;
        private readonly NumericUpDown quantity;
        private readonly TextBox address;
        private readonly Label total, count;
        private readonly Button send, update, remove;
        public string DeliveryAddress { get { return address.Text.Trim(); } }

        public CartForm(AppDatabase database, UserAccount user, ShoppingCart cart, string lastAddress)
        {
            this.database = database;
            this.user = user;
            this.cart = cart;
            SuspendLayout();
            Theme.Setup(this, "корзина", 900, 710);
            Controls.Add(Theme.Header("Ваша корзина", "Проверьте количество и адрес. Каждая позиция получит отдельный номер заказа.", 900));
            count = Theme.Label("", 26, 126, 848, 30, 11F);
            grid = Theme.Grid("gridCart", 26, 168, 848, 235);
            Theme.Column(grid, "ProductName", "Товар", 35);
            Theme.Column(grid, "Unit", "Ед.", 10);
            Theme.Column(grid, "Quantity", "Количество", 18, "0.###");
            Theme.Column(grid, "UnitPrice", "Цена, ₽", 18, "N2");
            Theme.Column(grid, "TotalAmount", "Сумма, ₽", 19, "N2");
            grid.SelectionChanged += (s, e) => SelectItem();
            quantity = new NumericUpDown { Name = "numCartQuantity", AccessibleName = "Новое количество", Location = new Point(26, 425), Size = new Size(135, 26), DecimalPlaces = 3, Minimum = .001M, Maximum = 10000M, Value = 1M, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            update = Theme.Button("Изменить количество", 177, 418, 205, UpdateQuantity, true);
            remove = Theme.Button("Убрать товар", 398, 418, 170, RemoveItem, true);
            update.Anchor = remove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            var addressLabel = Theme.Label("Адрес доставки", 26, 476, 848, 25);
            addressLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            address = new TextBox { Name = "txtCartAddress", AccessibleName = "Адрес доставки", Location = new Point(26, 510), Size = new Size(848, 58), Multiline = true, MaxLength = 255, Text = lastAddress, ScrollBars = ScrollBars.Vertical, Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            total = Theme.Label("", 26, 591, 390, 40, 17F);
            total.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            send = Theme.Button("Оформить корзину", 650, 591, 224, Checkout);
            send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            var back = Theme.Button("Вернуться в каталог", 650, 644, 224, (s, e) => Close(), true);
            back.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            var hint = Theme.Label("Корзина хранится до выхода из каталога.", 26, 646, 590, 30);
            hint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            hint.ForeColor = Theme.Muted;
            Controls.AddRange(new Control[] { count, grid, quantity, update, remove, addressLabel, address, total, send, back, hint });
            CancelButton = back;
            Theme.Apply(this);
            RefreshCart();
            ResumeLayout(false);
            PerformLayout();
        }

        private CartItem SelectedItem { get { return grid.CurrentRow == null ? null : grid.CurrentRow.DataBoundItem as CartItem; } }
        private void SelectItem()
        {
            var item = SelectedItem;
            if (quantity != null && item != null) quantity.Value = item.Quantity;
            if (update != null) update.Enabled = item != null;
            if (remove != null) remove.Enabled = item != null;
        }
        private void RefreshCart()
        {
            grid.DataSource = cart.Items.ToList();
            count.Text = cart.Items.Count == 0 ? "Корзина пуста. Добавьте товары из каталога." : "Позиций в корзине: " + cart.Items.Count;
            total.Text = "Итого: " + Theme.Money(cart.TotalAmount);
            send.Enabled = cart.Items.Count > 0;
            SelectItem();
        }
        private void UpdateQuantity(object sender, EventArgs e)
        {
            var item = SelectedItem;
            if (item == null) return;
            try { cart.SetQuantity(item.Product, quantity.Value); RefreshCart(); }
            catch (Exception exception) { UiErrors.Show(this, exception); }
        }
        private void RemoveItem(object sender, EventArgs e)
        {
            if (SelectedItem != null) cart.Remove(SelectedItem);
            RefreshCart();
        }
        private void Checkout(object sender, EventArgs e)
        {
            if (cart.Items.Count == 0) return;
            send.Enabled = false;
            UseWaitCursor = true;
            try
            {
                var ids = database.CreateOrders(user, cart.Items, address.Text);
                string receipt = "Номера заказов: " + string.Join(", ", ids) + "\nИтого: " + Theme.Money(cart.TotalAmount);
                cart.Clear();
                MessageBox.Show(this, receipt + "\nЗаказы доступны в истории.", "Корзина оформлена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception) { UiErrors.Show(this, exception); }
            finally { UseWaitCursor = false; if (!IsDisposed) send.Enabled = cart.Items.Count > 0; }
        }
    }
}
