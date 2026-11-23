using System;
using System.Windows.Forms;

namespace SupplierApp
{
    public partial class OrdersForm : Form
    {
        private readonly AppDatabase database;
        private readonly UserAccount currentUser;
        public OrdersForm() { InitializeComponent(); Theme.Apply(this); }
        public OrdersForm(AppDatabase database, UserAccount user) : this()
        {
            this.database = database;
            currentUser = user;
            txtDatabasePath.Text = database.DatabasePath;
            lblTitle.Text = "Мои заказы — " + user.FullName;
        }

        private void OrdersForm_Shown(object sender, EventArgs e) { LoadOrders(); }

        private void LoadOrders()
        {
            if (database == null || currentUser == null) return;
            try
            {
                var table = database.GetOrders(currentUser.UserId);
                gridOrders.DataSource = table;
                string[] names = { "OrderId", "ProductName", "Quantity", "UnitPrice", "TotalAmount", "DeliveryAddress", "CreatedAt", "OrderStatus" };
                string[] headers = { "№ заказа", "Товар", "Кол-во, кг", "Цена, руб.", "Сумма, руб.", "Адрес доставки", "Дата заказа", "Статус" };
                int[] weights = { 55, 100, 70, 80, 85, 170, 110, 80 };
                for (int index = 0; index < names.Length; index++)
                {
                    var column = gridOrders.Columns[names[index]];
                    column.HeaderText = headers[index];
                    column.FillWeight = weights[index];
                }
                gridOrders.Columns["Quantity"].DefaultCellStyle.Format = "0.###";
                gridOrders.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
                gridOrders.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
                gridOrders.Columns["CreatedAt"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                lblCount.Text = table.Rows.Count == 0 ? "У вас пока нет заказов." : "Всего заказов: " + table.Rows.Count;
            }
            catch (Exception exception)
            {
                gridOrders.DataSource = null;
                lblCount.Text = "Не удалось загрузить заказы.";
                UiErrors.Show(this, exception);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) { LoadOrders(); }
        private void btnClose_Click(object sender, EventArgs e) { Close(); }
    }
}
