using System;
using System.Collections.Generic;
using System.Linq;

namespace SupplierApp
{
    public sealed class CartItem
    {
        public Product Product { get; private set; }
        public string ProductName { get { return Product.ProductName; } }
        public string Unit { get { return Product.Unit; } }
        public decimal UnitPrice { get { return Product.UnitPrice; } }
        public decimal Quantity { get; private set; }
        public decimal TotalAmount { get { return decimal.Round(UnitPrice * Quantity, 2, MidpointRounding.ToEven); } }

        public CartItem(Product product, decimal quantity)
        {
            if (product == null) throw new ArgumentNullException("product");
            InputValidation.Order(quantity, "Корзина");
            Product = product;
            Quantity = quantity;
        }
    }

    public sealed class ShoppingCart
    {
        private readonly List<CartItem> items = new List<CartItem>();
        public IList<CartItem> Items { get { return items.AsReadOnly(); } }
        public decimal TotalAmount { get { return items.Sum(item => item.TotalAmount); } }

        public void Add(Product product, decimal quantity)
        {
            if (product == null) throw new ArgumentNullException("product");
            var existing = items.FirstOrDefault(item => item.Product.ProductId == product.ProductId);
            if (existing != null && existing.UnitPrice != product.UnitPrice)
                throw new ArgumentException("Цена изменилась. Удалите прежнюю позицию из корзины и добавьте товар заново.");
            SetQuantity(product, quantity + (existing == null ? 0 : existing.Quantity));
        }

        public void SetQuantity(Product product, decimal quantity)
        {
            var replacement = new CartItem(product, quantity);
            int index = items.FindIndex(item => item.Product.ProductId == product.ProductId);
            if (index < 0) items.Add(replacement);
            else items[index] = replacement;
        }

        public void Remove(CartItem item) { items.Remove(item); }
        public void Clear() { items.Clear(); }
    }
}
