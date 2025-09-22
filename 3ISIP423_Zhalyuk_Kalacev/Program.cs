using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ShopInventory
{
   public enum Category
    {
        Электроника,
        Одежда,
        Продукты
    }
    public class Product
    {
        public string Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public int Quantity { get; private set; }
        public bool IsAvailable => Quantity > 0;
        public Category Category { get; }
        public Product(string name, decimal price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым.");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть положительной.");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");

            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }
        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Количество должно быть положительным.");
            Quantity += amount;
        }
        public void DecreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Количество должно быть положительным.");
            if (Quantity < amount)
                throw new InvalidOperationException("Недостаточно товара на складе.");
            Quantity -= amount;
        }
        public override string ToString()
        {
            return $"Код: {Id} | Название: {Name} | Цена: {Price:C} | Количество: {Quantity} | Наличие: {(IsAvailable ? "Да" : "Нет")} | Категория: {Category}";
        }
    }
    public class ProductManager
    {
        private List<Product> _products = new List<Product>();
        private int _nextId = 1;
        public void AddProduct(string name, decimal price, int quantity, Category category)
        {
            var product = new Product(name, price, quantity, category);
            product.GetType().GetProperty("Id")?.SetValue(product, _nextId++.ToString());
            _products.Add(product);
        }
        public bool RemoveProduct(string id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            _products.Remove(product);
            return true;
        }

        public void SupplyProduct(string id, int amount)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new ArgumentException("Товар не найден.");
            product.IncreaseQuantity(amount);
        }

        public void SellProduct(string id, int amount)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new ArgumentException("Товар не найден.");
            product.DecreaseQuantity(amount);
        }
        public Product FindByCode(string id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable FindByName(string name)
        {
            return _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable FindByCategory(Category category)
        {
            return _products.Where(p => p.Category == category);
        }
    }
}

