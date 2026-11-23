using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    partial class CatalogForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblUser, lblCount, lblNotice;
        private TextBox txtSearch;
        private ComboBox cmbSort;
        private DataGridView gridProducts;
        private NumericUpDown numQuantity;
        private Button btnOrder, btnCart;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            SuspendLayout();
            Theme.Setup(this, "каталог", 1000, 690);
            StartPosition = FormStartPosition.CenterScreen;
            Controls.Add(Theme.Header("Свежие продукты к вашему столу", "Выберите товары, добавьте в корзину и укажите адрес доставки.", 1000));
            lblUser = Theme.Label("Добро пожаловать!", 26, 126, 580, 28, 11F);
            var orders = Theme.Button("Мои заказы", 648, 126, 155, btnOrders_Click, true);
            var exit = Theme.Button("Выйти", 815, 126, 159, btnExit_Click, true);
            orders.Anchor = exit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtSearch = new TextBox { Name = "txtSearch", AccessibleName = "Поиск товара", Location = new Point(26, 205), Size = new Size(575, 26), MaxLength = 100, TabIndex = 0 };
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            cmbSort = new ComboBox { Name = "cmbSort", AccessibleName = "Сортировка", Location = new Point(620, 205), Size = new Size(198, 26), DropDownStyle = ComboBoxStyle.DropDownList, TabIndex = 1 };
            cmbSort.Items.AddRange(new object[] { "По названию", "Сначала дешевле", "Сначала дороже" });
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += (s, e) => ApplyFilter();
            cmbSort.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            var refresh = Theme.Button("Обновить", 834, 198, 140, btnRefresh_Click, true);
            refresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            gridProducts = Theme.Grid("gridProducts", 26, 282, 948, 235);
            Theme.Column(gridProducts, "ProductName", "Наименование", 55);
            Theme.Column(gridProducts, "Unit", "Единица", 15);
            Theme.Column(gridProducts, "UnitPrice", "Цена, ₽", 30, "N2");
            gridProducts.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) btnOrder_Click(s, e); };
            gridProducts.TabIndex = 2;
            lblCount = Theme.Label("Загрузка каталога…", 26, 245, 948, 26);
            lblNotice = Theme.Label("Добавляйте несколько товаров — всё можно проверить в корзине.", 26, 525, 948, 30);
            lblNotice.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblNotice.ForeColor = Theme.Muted;
            var quantityLabel = Theme.Label("Количество", 26, 570, 110, 26);
            quantityLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            numQuantity = new NumericUpDown { Name = "numQuantity", AccessibleName = "Количество товара", Location = new Point(140, 573), Size = new Size(110, 26), DecimalPlaces = 3, Minimum = .001M, Maximum = 10000M, Value = 1M, Increment = .5M, TabIndex = 3, Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
            btnOrder = Theme.Button("Добавить в корзину", 268, 566, 200, btnOrder_Click);
            btnOrder.TabIndex = 4;
            btnOrder.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCart = Theme.Button("Корзина", 594, 566, 380, OpenCart);
            btnCart.TabIndex = 5;
            btnCart.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            var footer = Theme.Label("SUPPLIERAPP 4.0   /   Учебный магазин   ·   Цены учебные", 26, 635, 948, 24);
            footer.ForeColor = Theme.Muted;
            footer.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            var sortLabel = Theme.Label("Сортировка", 620, 175, 198, 23);
            sortLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Controls.AddRange(new Control[] { lblUser, orders, exit, Theme.Label("Поиск по названию", 26, 175, 300, 23), sortLabel, txtSearch, cmbSort, refresh, lblCount, gridProducts, lblNotice, quantityLabel, numQuantity, btnOrder, btnCart, footer });
            CancelButton = exit;
            Shown += CatalogForm_Shown;
            FormClosing += ClosingCatalog;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
