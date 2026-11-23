using System;
using System.Windows.Forms;

namespace SupplierApp
{
    internal static class UiErrors
    {
        public static void Show(IWin32Window owner, Exception exception)
        {
            bool validation = exception is ArgumentException;
            string text = validation ? exception.Message :
                "Операция с базой данных не выполнена.\n\n" + exception.Message +
                "\n\nПроверьте доступ к файлу базы и закройте его монопольное открытие в Access.";
            MessageBox.Show(owner, text, validation ? "Проверьте данные" : "Ошибка базы данных",
                MessageBoxButtons.OK, validation ? MessageBoxIcon.Warning : MessageBoxIcon.Error);
        }
    }
}
