using System.Drawing;
using System.Windows.Forms;

namespace SupplierApp
{
    internal static class Theme
    {
        public static readonly Color Ink = Color.FromArgb(28, 48, 62);
        public static readonly Color Muted = Color.FromArgb(93, 112, 122);
        public static readonly Color Accent = Color.FromArgb(19, 119, 96);
        public static readonly Color Canvas = Color.FromArgb(243, 247, 246);
        public static readonly Color Line = Color.FromArgb(220, 231, 226);

        public static void Apply(Form form)
        {
            form.BackColor = Canvas;
            form.ForeColor = Ink;
            form.Icon = SystemIcons.Application;
            StyleChildren(form);
        }

        private static void StyleChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                var button = control as Button;
                if (button != null)
                {
                    bool secondary = (string)button.Tag == "secondary";
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = secondary ? 1 : 0;
                    button.FlatAppearance.BorderColor = Line;
                    button.FlatAppearance.MouseOverBackColor = secondary ? Line : Color.FromArgb(15, 98, 79);
                    button.BackColor = secondary ? Color.White : Accent;
                    button.ForeColor = secondary ? Ink : Color.White;
                    button.Cursor = Cursors.Hand;
                    button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    button.UseVisualStyleBackColor = false;
                }
                var label = control as Label;
                if (label != null && label.Font.Size >= 12 && label.Tag == null)
                {
                    label.Font = new Font("Segoe UI", label.Font.Size, FontStyle.Bold);
                    label.ForeColor = Ink;
                }
                var grid = control as DataGridView;
                if (grid != null)
                {
                    grid.BackgroundColor = Color.White;
                    grid.BorderStyle = BorderStyle.None;
                    grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    grid.GridColor = Line;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(231, 240, 235);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Ink;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8);
                    grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    grid.ColumnHeadersHeight = 42;
                    grid.DefaultCellStyle.Padding = new Padding(8, 3, 8, 3);
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(213, 237, 225);
                    grid.DefaultCellStyle.SelectionForeColor = Ink;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 249);
                    grid.RowTemplate.Height = 42;
                }
                StyleChildren(control);
            }
        }

        public static void Setup(Form form, string title, int width, int height)
        {
            form.Text = "SupplierApp — " + title;
            form.Font = new Font("Segoe UI", 9F);
            form.AutoScaleDimensions = new SizeF(96F, 96F);
            form.AutoScaleMode = AutoScaleMode.Dpi;
            form.ClientSize = new Size(width, height);
            form.MinimumSize = form.Size;
            form.StartPosition = FormStartPosition.CenterParent;
        }

        public static void AddHeader(Form form, Label oldTitle, string title, string subtitle)
        {
            form.SuspendLayout();
            form.Controls.Remove(oldTitle);
            oldTitle.Dispose();
            foreach (Control control in form.Controls) control.Top += 60;
            form.ClientSize = new Size(form.ClientSize.Width, form.ClientSize.Height + 60);
            form.Controls.Add(Header(title, subtitle, form.ClientSize.Width));
            form.ResumeLayout(false);
            form.PerformLayout();
        }

        public static Label Label(string text, int x, int y, int width, int height, float size = 9F)
        {
            return new Label { Text = text, Location = new Point(x, y), Size = new Size(width, height),
                Font = new Font("Segoe UI", size), AutoEllipsis = true, TextAlign = ContentAlignment.MiddleLeft };
        }

        public static Button Button(string text, int x, int y, int width, System.EventHandler handler, bool secondary = false)
        {
            var button = new Button { Text = text, Location = new Point(x, y), Size = new Size(width, 38),
                Tag = secondary ? "secondary" : null, AccessibleName = text };
            button.Click += handler;
            return button;
        }

        public static Panel Header(string title, string subtitle, int width)
        {
            var panel = new Panel { Location = Point.Empty, Size = new Size(width, 112),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            var brand = Label("ВАШ ЛЮБИМЫЙ ПОСТАВЩИК  /  04", 26, 12, width - 52, 22);
            brand.ForeColor = Color.FromArgb(149, 213, 180);
            brand.Tag = "header";
            var heading = Label(title, 24, 37, width - 48, 36, 20F);
            heading.ForeColor = Color.White;
            heading.Tag = "header";
            heading.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            var caption = Label(subtitle, 26, 78, width - 52, 24);
            caption.ForeColor = Color.FromArgb(201, 220, 214);
            caption.Tag = "header";
            foreach (var label in new[] { brand, heading, caption })
                label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel.Controls.AddRange(new Control[] { brand, heading, caption });
            return panel;
        }

        public static DataGridView Grid(string name, int x, int y, int width, int height)
        {
            return new DataGridView { Name = name, Location = new Point(x, y), Size = new Size(width, height),
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, MultiSelect = false, RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
        }

        public static void Column(DataGridView grid, string property, string title, int weight, string format = null)
        {
            var column = new DataGridViewTextBoxColumn { Name = property, DataPropertyName = property,
                HeaderText = title, FillWeight = weight, MinimumWidth = 65, SortMode = DataGridViewColumnSortMode.NotSortable };
            if (format != null) column.DefaultCellStyle.Format = format;
            grid.Columns.Add(column);
        }

        public static string Money(decimal value)
        {
            return value.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("ru-RU")) + " ₽";
        }
    }
}
