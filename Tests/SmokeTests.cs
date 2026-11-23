using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SupplierApp;

internal static class SmokeTests
{
    private static int passed;
    private static void Check(bool result, string message)
    {
        if (!result) throw new Exception(message);
        Console.WriteLine("PASS: " + message);
        passed++;
    }
    private static void Invalid(Action action, string message)
    {
        try { action(); } catch (ArgumentException) { Check(true, message); return; }
        throw new Exception("Expected validation error: " + message);
    }
    [STAThread]
    private static int Main(string[] args)
    {
        try
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string fixture = Path.Combine(folder, "Fixture.mdb");
            if (args.Length > 0 && (args[0] == "--preview" || args[0] == "--login"))
            {
                var preview = new AppDatabase(fixture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (args[0] == "--login") Application.Run(new LoginForm(preview));
                else
                {
                    var account = preview.Authenticate("student", "Student123!");
                    var products = preview.GetProducts();
                    preview.CreateOrder(account, products[0], 2.5M, "Учебный адрес, дом 10");
                    preview.CreateOrder(account, products[1], 1M, "Учебный адрес, дом 10");
                    Application.Run(new CatalogForm(preview, account));
                }
                return 0;
            }
            if (args.Length != 1) throw new ArgumentException("Pass the path to the source SupplierApp.mdb.");
            File.Copy(args[0], fixture, true);
            var db = new AppDatabase(fixture);
            db.CheckConnection();
            Check(true, "Jet OleDb x86 opens all three tables on a database copy");
            var student = db.Authenticate(" STUDENT ", "Student123!");
            Check(student != null, "Student login and username normalization");
            Check(db.Authenticate("student", "WrongPassword!") == null, "Wrong password rejected");
            var user = db.Register("smoke_" + Guid.NewGuid().ToString("N").Substring(0, 10), "Проверка версии 4", "SmokePass123!");
            Check(db.Authenticate(user.UserName, "SmokePass123!") != null, "Registration and login persisted");
            Invalid(() => db.Register(user.UserName, "Дубликат", "SmokePass123!"), "Duplicate username rejected");
            var productsList = db.GetProducts();
            Check(productsList.Count >= 2, "Catalog loads active products");
            var first = productsList[0]; var second = productsList[1];
            var cart = new ShoppingCart();
            cart.Add(first, 1.125M); cart.Add(first, .375M); cart.Add(second, 2M);
            Check(cart.Items.Count == 2 && cart.Items[0].Quantity == 1.5M, "Repeated product merged, fractional quantity preserved");
            Check(cart.TotalAmount == decimal.Round(first.UnitPrice * 1.5M, 2, MidpointRounding.ToEven) + second.UnitPrice * 2M, "Cart total uses per-line rounding");
            Invalid(() => cart.Add(first, 10000M), "Quantity overflow rejected without modifying cart");
            Invalid(() => new CartItem(first, .0001M), "Quantity precision validated");
            Invalid(() => db.CreateOrders(user, cart.Items, "x"), "Invalid address rejected");
            Check(db.GetOrders(user.UserId).Rows.Count == 0, "Validation saved no partial orders");
            var stale = new ShoppingCart();
            stale.Add(first, 1M);
            stale.Add(new Product(second.ProductId, second.ProductName, second.Unit, second.UnitPrice + 1M), 1M);
            Invalid(() => db.CreateOrders(user, stale.Items, "Учебный адрес, 10"), "Changed price rejected on second basket line");
            Check(db.GetOrders(user.UserId).Rows.Count == 0, "Transaction rolled back first basket line");
            var inactive = new ShoppingCart();
            inactive.Add(first, 1M);
            inactive.Add(new Product(int.MaxValue, "Недоступный", "кг", 1M), 1M);
            Invalid(() => db.CreateOrders(user, inactive.Items, "Учебный адрес, 10"), "Unavailable product rejected");
            Check(db.GetOrders(user.UserId).Rows.Count == 0, "Unavailable product rolled back the full basket");
            var ids = db.CreateOrders(user, cart.Items, "  Учебный адрес, 10  ");
            var history = db.GetOrders(user.UserId);
            Check(ids.Count == 2 && ids[0] != ids[1] && history.Rows.Count == 2, "All basket lines persisted with unique IDs");
            decimal savedTotal = 0;
            foreach (DataRow row in history.Rows) savedTotal += Convert.ToDecimal(row["TotalAmount"]);
            Check(savedTotal == cart.TotalAmount && Convert.ToString(history.Rows[0]["DeliveryAddress"]) == "Учебный адрес, 10", "Saved total and trimmed address match checkout");
            Check(!db.CancelOrder(student.UserId, ids[0]), "Other user cannot cancel this order");
            Check(db.CancelOrder(user.UserId, ids[0]), "Owner can cancel a new order");
            Check(!db.CancelOrder(user.UserId, ids[0]), "Cancelled order cannot be cancelled again");
            history = db.GetOrders(user.UserId);
            Check(history.Select("OrderStatus = 'Отменён'").Length == 1, "Cancellation retained order in history");
            history.Rows[0]["DeliveryAddress"] = "=1+1;\"тест\"\r\nстрока";
            history.DefaultView.RowFilter = "OrderStatus = 'Новый'";
            string csv = Path.Combine(folder, "Orders.csv");
            OrderExport.Save(history.DefaultView, csv);
            string exported = File.ReadAllText(csv);
            Check(exported.Contains("'=1+1") && exported.Contains("\"\"тест\"\""), "CSV neutralizes formulas and escapes quotes");
            Check(!exported.Contains("Отменён"), "CSV respects status filter");
            using (var reopened = new MemoryStream(File.ReadAllBytes(csv)))
                Check(reopened.ReadByte() == 239 && reopened.ReadByte() == 187 && reopened.ReadByte() == 191, "CSV has UTF-8 BOM for Cyrillic Excel");
            cart.Remove(cart.Items[0]);
            Check(cart.Items.Count == 1, "Removing a cart line");
            cart.Clear();
            Check(cart.Items.Count == 0 && cart.TotalAmount == 0, "Clearing a cart");
            Console.WriteLine("SUCCESS: " + passed + " checks passed. Original database was not modified.");
            return 0;
        }
        catch (Exception exception) { Console.Error.WriteLine(exception); return 1; }
    }
}
