using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    partial class OrdersForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblOwner, lblCount;
        private ComboBox cmbStatus;
        private DataGridView gridOrders;
        private Button btnDetails, btnCancelOrder, btnExport;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            SuspendLayout();
            Theme.Setup(this, "мои заказы", 1100, 660);
            Controls.Add(Theme.Header("Мои заказы", "Все покупки в одном месте. Откройте карточку, сохраните список или отмените новый заказ.", 1100));
            lblOwner = Theme.Label("История покупок", 26, 128, 600, 30, 11F);
            cmbStatus = new ComboBox { Name = "cmbStatus", AccessibleName = "Фильтр по статусу", Location = new Point(768, 132), Size = new Size(180, 26), DropDownStyle = ComboBoxStyle.DropDownList, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            cmbStatus.Items.AddRange(new object[] { "Все статусы", "Новые", "Отменённые" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilter();
            var refresh = Theme.Button("Обновить", 960, 126, 114, btnRefresh_Click, true);
            refresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gridOrders = Theme.Grid("gridOrders", 26, 181, 1048, 316);
            Theme.Column(gridOrders, "OrderId", "№", 7);
            Theme.Column(gridOrders, "ProductName", "Товар", 20);
            Theme.Column(gridOrders, "Quantity", "Кол-во", 12, "0.###");
            Theme.Column(gridOrders, "Unit", "Ед.", 7);
            Theme.Column(gridOrders, "UnitPrice", "Цена, ₽", 13, "N2");
            Theme.Column(gridOrders, "TotalAmount", "Сумма, ₽", 15, "N2");
            Theme.Column(gridOrders, "CreatedAt", "Дата", 21, "dd.MM.yyyy HH:mm");
            Theme.Column(gridOrders, "OrderStatus", "Статус", 14);
            gridOrders.SelectionChanged += (s, e) => SelectionChanged();
            gridOrders.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Details(s, e); };
            gridOrders.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == gridOrders.Columns["OrderStatus"].Index && e.Value != null)
                    e.CellStyle.ForeColor = e.Value.ToString() == "Новый" ? Theme.Accent : Theme.Muted;
            };
            lblCount = Theme.Label("Загрузка…", 26, 510, 1048, 32, 11F);
            lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnDetails = Theme.Button("Открыть заказ", 26, 563, 174, Details);
            btnCancelOrder = Theme.Button("Отменить заказ", 216, 563, 180, CancelOrder, true);
            btnExport = Theme.Button("Сохранить CSV", 412, 563, 180, Export, true);
            var close = Theme.Button("В каталог", 900, 563, 174, btnClose_Click, true);
            btnDetails.Anchor = btnCancelOrder.Anchor = btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            close.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            var hint = Theme.Label("Отменить можно только заказ со статусом «Новый». Отменённые заказы остаются в истории.", 26, 615, 1048, 25);
            hint.ForeColor = Theme.Muted;
            hint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Controls.AddRange(new Control[] { lblOwner, cmbStatus, refresh, gridOrders, lblCount, btnDetails, btnCancelOrder, btnExport, close, hint });
            CancelButton = close;
            Shown += OrdersForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
