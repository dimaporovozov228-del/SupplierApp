using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    internal static class Theme
    {
        public static void Apply(Form form)
        {
            form.BackColor = Color.FromArgb(244, 247, 251);
            form.ForeColor = Color.FromArgb(30, 47, 67);
            foreach (Control control in form.Controls)
            {
                var button = control as Button;
                if (button != null)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.BackColor = Color.FromArgb(29, 91, 130);
                    button.ForeColor = Color.White;
                    button.Cursor = Cursors.Hand;
                    button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                var label = control as Label;
                if (label != null && label.Font.Size >= 12)
                {
                    label.Font = new Font("Segoe UI", label.Font.Size, FontStyle.Bold);
                    label.ForeColor = Color.FromArgb(19, 72, 110);
                }
                var grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(29, 91, 130);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(237, 244, 250);
                }
            }
        }
    }
}
