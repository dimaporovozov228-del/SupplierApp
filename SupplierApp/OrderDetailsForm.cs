using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    public sealed class OrderDetailsForm : Form
    {
        public OrderDetailsForm(DataRow row)
        {
            Theme.Setup(this, "карточка заказа", 620, 570);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Controls.Add(Theme.Header("Заказ № " + row["OrderId"], "От " + Convert.ToDateTime(row["CreatedAt"]).ToString("dd.MM.yyyy HH:mm"), 620));
            Controls.Add(Theme.Label(Convert.ToString(row["ProductName"]), 26, 135, 568, 42, 19F));
            var status = Theme.Label("Статус: " + row["OrderStatus"], 26, 188, 568, 36, 11F);
            status.BackColor = Color.FromArgb(225, 239, 231);
            Controls.Add(status);
            Controls.Add(Theme.Label("Количество: " + Convert.ToDecimal(row["Quantity"]).ToString("0.###") + " " + row["Unit"], 26, 240, 568, 30, 11F));
            Controls.Add(Theme.Label("Цена за единицу: " + Theme.Money(Convert.ToDecimal(row["UnitPrice"])), 26, 277, 568, 30, 11F));
            Controls.Add(Theme.Label("Сумма: " + Theme.Money(Convert.ToDecimal(row["TotalAmount"])), 26, 318, 568, 40, 17F));
            Controls.Add(Theme.Label("Адрес доставки", 26, 376, 568, 25));
            Controls.Add(new TextBox { Location = new Point(26, 411), Size = new Size(568, 75), Multiline = true, ReadOnly = true, Text = Convert.ToString(row["DeliveryAddress"]), ScrollBars = ScrollBars.Vertical, AccessibleName = "Адрес доставки" });
            var close = Theme.Button("Закрыть", 414, 506, 180, (s, e) => Close(), true);
            Controls.Add(close);
            CancelButton = close;
            Theme.Apply(this);
        }
    }
}
