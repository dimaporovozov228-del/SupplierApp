using System.Globalization;

namespace SupplierApp
{
    public sealed class UserAccount
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public string FullName { get; private set; }

        public UserAccount(int userId, string userName, string fullName)
        {
            UserId = userId;
            UserName = userName;
            FullName = fullName;
        }
    }

    public sealed class Product
    {
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string Unit { get; private set; }
        public decimal UnitPrice { get; private set; }
        public string DisplayName
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("ru-RU"),
                    "{0} — {1:N2} руб. / {2}", ProductName, UnitPrice, Unit);
            }
        }

        public Product(int productId, string productName, string unit, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Unit = unit;
            UnitPrice = unitPrice;
        }
    }
}
