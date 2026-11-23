using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace SupplierApp
{
    // Единственная точка работы с Access. Все пользовательские значения передаются параметрами.
    public sealed class AppDatabase
    {
        private readonly string connectionString;
        public string DatabasePath { get; private set; }

        public AppDatabase(string databasePath)
        {
            if (string.IsNullOrWhiteSpace(databasePath)) throw new ArgumentException("Не указан путь к базе.");
            DatabasePath = Path.GetFullPath(databasePath);
            var setting = ConfigurationManager.ConnectionStrings["SupplierDb"];
            string configured = setting == null
                ? "Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Mode=Share Deny None;"
                : setting.ConnectionString;
            var builder = new OleDbConnectionStringBuilder(configured);
            builder.DataSource = DatabasePath;
            connectionString = builder.ConnectionString;
        }

        private OleDbConnection OpenConnection()
        {
            if (!File.Exists(DatabasePath))
                throw new FileNotFoundException("Не найден файл базы данных: " + DatabasePath, DatabasePath);
            var connection = new OleDbConnection(connectionString);
            try { connection.Open(); return connection; }
            catch { connection.Dispose(); throw; }
        }

        public void CheckConnection()
        {
            using (var connection = OpenConnection())
            {
                string[] queries =
                {
                    "SELECT TOP 1 [UserId], [UserName], [FullName], [PasswordSalt], [PasswordHash], [HashIterations], [CreatedAt] FROM [Users]",
                    "SELECT TOP 1 [ProductId], [ProductName], [Unit], [UnitPrice], [IsActive] FROM [Products]",
                    "SELECT TOP 1 [OrderId], [UserId], [ProductId], [Quantity], [UnitPrice], [DeliveryAddress], [CreatedAt], [OrderStatus] FROM [Orders]"
                };
                foreach (string query in queries)
                    using (var command = new OleDbCommand(query, connection))
                    using (var reader = command.ExecuteReader()) { }
            }
        }

        public UserAccount Authenticate(string userName, string password)
        {
            string normalized = InputValidation.NormalizeUserName(userName);
            if (normalized.Length == 0 || normalized.Length > 50 || string.IsNullOrEmpty(password) || password.Length > 128)
                return null;
            using (var connection = OpenConnection())
            using (var command = new OleDbCommand(
                "SELECT [UserId], [UserName], [FullName], [PasswordSalt], [PasswordHash], [HashIterations] FROM [Users] WHERE [UserName] = ?", connection))
            {
                command.Parameters.Add("pUserName", OleDbType.VarWChar, 50).Value = normalized;
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    bool valid = PasswordHasher.Verify(password, reader.GetString(3), reader.GetString(4), reader.GetInt32(5));
                    return valid ? new UserAccount(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)) : null;
                }
            }
        }

        public UserAccount Register(string userName, string fullName, string password)
        {
            InputValidation.Registration(userName, fullName, password);
            string normalized = InputValidation.NormalizeUserName(userName);
            string name = fullName.Trim();
            string salt, hash;
            int iterations;
            PasswordHasher.Create(password, out salt, out hash, out iterations);
            try
            {
                using (var connection = OpenConnection())
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new OleDbCommand(
                        "INSERT INTO [Users] ([UserName], [FullName], [PasswordSalt], [PasswordHash], [HashIterations], [CreatedAt]) VALUES (?, ?, ?, ?, ?, ?)", connection, transaction))
                    {
                        // OleDb учитывает порядок, а не имена параметров.
                        command.Parameters.Add("pUserName", OleDbType.VarWChar, 50).Value = normalized;
                        command.Parameters.Add("pFullName", OleDbType.VarWChar, 100).Value = name;
                        command.Parameters.Add("pSalt", OleDbType.VarWChar, 24).Value = salt;
                        command.Parameters.Add("pHash", OleDbType.VarWChar, 44).Value = hash;
                        command.Parameters.Add("pIterations", OleDbType.Integer).Value = iterations;
                        command.Parameters.Add("pCreatedAt", OleDbType.Date).Value = DateTime.Now;
                        if (command.ExecuteNonQuery() != 1) throw new DataException("Учетная запись не сохранена.");
                    }
                    int userId;
                    using (var identity = new OleDbCommand("SELECT @@IDENTITY", connection, transaction))
                        userId = Convert.ToInt32(identity.ExecuteScalar());
                    transaction.Commit();
                    return new UserAccount(userId, normalized, name);
                }
            }
            catch (OleDbException exception)
            {
                foreach (OleDbError error in exception.Errors)
                    if (error.NativeError == 3022 || error.SQLState == "3022")
                        throw new ArgumentException("Этот логин уже занят. Выберите другой.", exception);
                throw;
            }
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            using (var connection = OpenConnection())
            using (var command = new OleDbCommand(
                "SELECT [ProductId], [ProductName], [Unit], [UnitPrice] FROM [Products] WHERE [IsActive] = True ORDER BY [ProductId]", connection))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    products.Add(new Product(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDecimal(3)));
            return products;
        }

        private int InsertOrder(OleDbConnection connection, OleDbTransaction transaction, UserAccount user, Product product, decimal quantity, string deliveryAddress)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (product == null) throw new ArgumentNullException("product");
            using (var check = new OleDbCommand(
                "SELECT [UnitPrice] FROM [Products] WHERE [ProductId] = ? AND [IsActive] = True", connection, transaction))
            {
                check.Parameters.Add("pProductId", OleDbType.Integer).Value = product.ProductId;
                object currentPrice = check.ExecuteScalar();
                if (currentPrice == null || currentPrice == DBNull.Value)
                    throw new ArgumentException("Товар больше не доступен. Обновите каталог.");
                if (Convert.ToDecimal(currentPrice) <= 0)
                    throw new ArgumentException("В каталоге указана некорректная цена. Исправьте ее в базе данных.");
                if (Convert.ToDecimal(currentPrice) != product.UnitPrice)
                    throw new ArgumentException("Цена товара изменилась. Удалите эту позицию из корзины, обновите каталог и добавьте товар заново.");
            }
            using (var command = new OleDbCommand(
                "INSERT INTO [Orders] ([UserId], [ProductId], [Quantity], [UnitPrice], [DeliveryAddress], [CreatedAt], [OrderStatus]) VALUES (?, ?, ?, ?, ?, ?, ?)", connection, transaction))
            {
                command.Parameters.Add("pUserId", OleDbType.Integer).Value = user.UserId;
                command.Parameters.Add("pProductId", OleDbType.Integer).Value = product.ProductId;
                // Jet 4.0 не принимает дробный Decimal-параметр для этого поля.
                // Currency точно хранит 4 знака; наша валидация разрешает не более 3.
                var amount = command.Parameters.Add("pQuantity", OleDbType.Currency);
                amount.Value = quantity;
                command.Parameters.Add("pUnitPrice", OleDbType.Currency).Value = product.UnitPrice;
                command.Parameters.Add("pAddress", OleDbType.VarWChar, 255).Value = deliveryAddress;
                command.Parameters.Add("pCreatedAt", OleDbType.Date).Value = DateTime.Now;
                command.Parameters.Add("pStatus", OleDbType.VarWChar, 20).Value = "Новый";
                if (command.ExecuteNonQuery() != 1) throw new DataException("Заказ не сохранен.");
            }
            int orderId;
            using (var identity = new OleDbCommand("SELECT @@IDENTITY", connection, transaction))
                orderId = Convert.ToInt32(identity.ExecuteScalar());
            return orderId;
        }

        public int CreateOrder(UserAccount user, Product product, decimal quantity, string address)
        {
            return CreateOrders(user, new[] { new CartItem(product, quantity) }, address)[0];
        }

        // Вся корзина записывается одной транзакцией: ошибка любой позиции отменяет все записи.
        public List<int> CreateOrders(UserAccount user, IList<CartItem> items, string address)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (items == null || items.Count == 0) throw new ArgumentException("Корзина пуста.");
            string deliveryAddress = InputValidation.Order(1, address);
            foreach (var item in items)
            {
                if (item == null) throw new ArgumentException("Некорректная позиция корзины.");
                InputValidation.Order(item.Quantity, deliveryAddress);
            }
            using (var connection = OpenConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var ids = new List<int>();
                foreach (var item in items)
                    ids.Add(InsertOrder(connection, transaction, user, item.Product, item.Quantity, deliveryAddress));
                transaction.Commit();
                return ids;
            }
        }

        public bool CancelOrder(int userId, int orderId)
        {
            using (var connection = OpenConnection())
            using (var command = new OleDbCommand(
                "UPDATE [Orders] SET [OrderStatus] = ? WHERE [OrderId] = ? AND [UserId] = ? AND [OrderStatus] = ?", connection))
            {
                command.Parameters.Add("pCancelled", OleDbType.VarWChar, 20).Value = "Отменён";
                command.Parameters.Add("pOrderId", OleDbType.Integer).Value = orderId;
                command.Parameters.Add("pUserId", OleDbType.Integer).Value = userId;
                command.Parameters.Add("pNew", OleDbType.VarWChar, 20).Value = "Новый";
                return command.ExecuteNonQuery() == 1;
            }
        }

        public DataTable GetOrders(int userId)
        {
            using (var connection = OpenConnection())
            using (var command = new OleDbCommand(
                "SELECT o.[OrderId], p.[ProductName], p.[Unit], o.[Quantity], o.[UnitPrice], " +
                "Round(o.[Quantity] * o.[UnitPrice], 2) AS [TotalAmount], o.[DeliveryAddress], o.[CreatedAt], o.[OrderStatus] " +
                "FROM [Orders] AS o INNER JOIN [Products] AS p ON o.[ProductId] = p.[ProductId] " +
                "WHERE o.[UserId] = ? ORDER BY o.[OrderId] DESC", connection))
            {
                command.Parameters.Add("pUserId", OleDbType.Integer).Value = userId;
                using (var adapter = new OleDbDataAdapter(command))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }
    }
}
