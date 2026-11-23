using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class OrdersForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount currentUser;
        private DataTable orders;
        public OrdersForm() { InitializeComponent(); Theme.Apply(this); }
        public OrdersForm(AppDatabase database, UserAccount user) : this()
        {
            this.database = database;
            currentUser = user;
            lblOwner.Text = "История покупок · " + user.FullName;
        }
        private void OrdersForm_Shown(object sender, EventArgs e) { LoadOrders(); }
        private void LoadOrders()
        {
            if (database == null || currentUser == null) return;
            try { orders = database.GetOrders(currentUser.UserId); ApplyFilter(); }
            catch (Exception exception)
            {
                orders = null;
                gridOrders.DataSource = null;
                lblCount.Text = "Не удалось загрузить историю. Попробуйте обновить.";
                btnDetails.Enabled = btnCancelOrder.Enabled = btnExport.Enabled = false;
                UiErrors.Show(this, exception);
            }
        }
        private DataRow SelectedOrder
        {
            get
            {
                var row = gridOrders.CurrentRow == null ? null : gridOrders.CurrentRow.DataBoundItem as DataRowView;
                return row == null ? null : row.Row;
            }
        }
        private void ApplyFilter()
        {
            if (orders == null) return;
            orders.DefaultView.RowFilter = cmbStatus.SelectedIndex == 1 ? "OrderStatus = 'Новый'" : cmbStatus.SelectedIndex == 2 ? "OrderStatus = 'Отменён'" : "";
            gridOrders.DataSource = orders.DefaultView;
            decimal sum = 0;
            int active = 0;
            foreach (DataRowView row in orders.DefaultView)
                if (Convert.ToString(row["OrderStatus"]) != "Отменён") { sum += Convert.ToDecimal(row["TotalAmount"]); active++; }
            lblCount.Text = orders.DefaultView.Count == 0 ? "Заказов пока нет. Выберите товары в каталоге." : "Показано: " + orders.DefaultView.Count + "  ·  Без отменённых: " + active + "  ·  Сумма: " + Theme.Money(sum);
            btnExport.Enabled = orders.DefaultView.Count > 0;
            SelectionChanged();
        }
        private void SelectionChanged()
        {
            if (btnDetails == null) return;
            var row = SelectedOrder;
            btnDetails.Enabled = row != null;
            btnCancelOrder.Enabled = row != null && Convert.ToString(row["OrderStatus"]) == "Новый";
        }
        private void Details(object sender, EventArgs e)
        {
            if (SelectedOrder == null) return;
            using (var form = new OrderDetailsForm(SelectedOrder)) form.ShowDialog(this);
        }
        private void CancelOrder(object sender, EventArgs e)
        {
            var row = SelectedOrder;
            if (row == null || database == null || currentUser == null) return;
            int id = Convert.ToInt32(row["OrderId"]);
            if (MessageBox.Show(this, "Отменить заказ № " + id + " на товар «" + row["ProductName"] + "»?", "Отмена заказа", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                if (!database.CancelOrder(currentUser.UserId, id))
                    MessageBox.Show(this, "Статус уже изменился. Отменить можно только новый заказ.", "Отмена заказа");
                LoadOrders();
            }
            catch (Exception exception) { UiErrors.Show(this, exception); }
        }
        private void Export(object sender, EventArgs e)
        {
            if (orders == null || orders.DefaultView.Count == 0) return;
            using (var dialog = new SaveFileDialog { Filter = "Таблица CSV (*.csv)|*.csv", FileName = "Мои заказы " + DateTime.Now.ToString("yyyy-MM-dd") + ".csv", Title = "Сохранить показанные заказы", AddExtension = true })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try { OrderExport.Save(orders.DefaultView, dialog.FileName); MessageBox.Show(this, "Заказы сохранены. Файл можно открыть в Excel.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                catch (Exception exception) { MessageBox.Show(this, "Не удалось сохранить файл.\n\n" + exception.Message, "Ошибка экспорта", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e) { LoadOrders(); }
        private void btnClose_Click(object sender, EventArgs e) { Close(); }
    }
}
