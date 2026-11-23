using System;
using System.Windows.Forms;

namespace SupplierApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var context = new ApplicationContext();
            Form[] forms = { new LoginForm(), new RegisterForm(), new CatalogForm(), new OrderForm(), new OrdersForm() };
            int remaining = forms.Length;
            foreach (Form form in forms)
            {
                form.FormClosed += (sender, args) =>
                {
                    remaining--;
                    if (remaining == 0) context.ExitThread();
                };
                form.Show();
            }
            Application.Run(context);
        }
    }
}
