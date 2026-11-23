using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace SupplierApp
{
    public static class OrderExport
    {
        // Кавычки защищают разделители и переносы; апостроф — от формул при открытии в Excel.
        private static string Cell(object value)
        {
            string text = Convert.ToString(value, CultureInfo.GetCultureInfo("ru-RU"));
            string trimmed = text.TrimStart();
            if (trimmed.Length > 0 && "=+-@".IndexOf(trimmed[0]) >= 0) text = "'" + text;
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        public static void Save(DataView orders, string path)
        {
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                writer.WriteLine("Номер;Товар;Единица;Количество;Цена, руб.;Сумма, руб.;Адрес;Дата;Статус");
                string[] columns = { "OrderId", "ProductName", "Unit", "Quantity", "UnitPrice", "TotalAmount", "DeliveryAddress", "CreatedAt", "OrderStatus" };
                foreach (DataRowView row in orders)
                {
                    var cells = new string[columns.Length];
                    for (int i = 0; i < columns.Length; i++)
                        cells[i] = Cell(row[columns[i]] is DateTime ? ((DateTime)row[columns[i]]).ToString("dd.MM.yyyy HH:mm") : row[columns[i]]);
                    writer.WriteLine(string.Join(";", cells));
                }
            }
        }
    }
}
