using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SupplierApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
                AppDomain.CurrentDomain.SetData("DataDirectory", directory);
                var database = new AppDatabase(Path.Combine(directory, "SupplierApp.mdb"));
                database.CheckConnection();
                Application.Run(new LoginForm(database));
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Не удалось открыть базу данных SupplierApp.\n\n" + exception.Message +
                    "\n\nРаспакуйте весь архив в папку с правом записи. " +
                    "Используйте платформу x86 и Microsoft.Jet.OLEDB.4.0. " +
                    "Порядок запуска и настройка ACE описаны в README.md.",
                    "Ошибка запуска", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
