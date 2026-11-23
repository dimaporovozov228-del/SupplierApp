using System;
using System.Text.RegularExpressions;

namespace SupplierApp
{
    public static class InputValidation
    {
        public static string NormalizeUserName(string userName)
        {
            return (userName ?? string.Empty).Trim().ToLowerInvariant();
        }

        public static void Registration(string userName, string fullName, string password)
        {
            if (!Regex.IsMatch(NormalizeUserName(userName), @"\A[a-z0-9_.-]{3,50}\z"))
                throw new ArgumentException("Логин: от 3 до 50 латинских букв, цифр или символов _ . -.");
            string name = (fullName ?? string.Empty).Trim();
            if (name.Length < 2 || name.Length > 100)
                throw new ArgumentException("Укажите имя длиной от 2 до 100 символов.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Length > 128)
                throw new ArgumentException("Пароль должен содержать от 8 до 128 символов.");
        }

        public static string Order(decimal quantity, string address)
        {
            if (quantity <= 0 || quantity > 10000 || decimal.Round(quantity, 3) != quantity)
                throw new ArgumentException("Количество: больше 0 и не более 10000 кг, до трех знаков после запятой.");
            string value = (address ?? string.Empty).Trim();
            if (value.Length < 5 || value.Length > 255)
                throw new ArgumentException("Адрес должен содержать от 5 до 255 символов.");
            return value;
        }
    }
}
