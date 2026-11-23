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
                    if (error.NativeError == 3022)
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

        public int CreateOrder(UserAccount user, Product product, decimal quantity, string address)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (product == null) throw new ArgumentNullException("product");
            string deliveryAddress = InputValidation.Order(quantity, address);
            using (var connection = OpenConnection())
            using (var transaction = connection.BeginTransaction())
            {
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
                        throw new ArgumentException("Цена товара изменилась. Закройте заказ и обновите каталог.");
                }
                using (var command = new OleDbCommand(
                    "INSERT INTO [Orders] ([UserId], [ProductId], [Quantity], [UnitPrice], [DeliveryAddress], [CreatedAt], [OrderStatus]) VALUES (?, ?, ?, ?, ?, ?, ?)", connection, transaction))
                {
                    command.Parameters.Add("pUserId", OleDbType.Integer).Value = user.UserId;
                    command.Parameters.Add("pProductId", OleDbType.Integer).Value = product.ProductId;
                    var amount = command.Parameters.Add("pQuantity", OleDbType.Decimal);
                    amount.Precision = 18;
                    amount.Scale = 3;
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
                transaction.Commit();
                return orderId;
            }
        }

        public DataTable GetOrders(int userId)
        {
            using (var connection = OpenConnection())
            using (var command = new OleDbCommand(
                "SELECT o.[OrderId], p.[ProductName], o.[Quantity], o.[UnitPrice], " +
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
