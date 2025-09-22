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
        public void AddTestData()
        {
            AddProduct("Ноутбук", 50000, 10, Category.Электроника);
            AddProduct("Футболка", 1500, 50, Category.Одежда);
            AddProduct("Яблоки", 100, 200, Category.Продукты);
            AddProduct("Наушники", 3000, 30, Category.Электроника);
            AddProduct("Джинсы", 2500, 40, Category.Одежда);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ProductManager();
            manager.AddTestData();

            while (true)
            {
                Console.WriteLine("\n1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товара");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");
            }
        }
        static void AddProduct(ProductManager manager)
        {
            try
            {
                Console.Write("Название: ");
                var name = Console.ReadLine();
                Console.Write("Цена: ");
                var price = decimal.Parse(Console.ReadLine());
                Console.Write("Количество: ");
                var quantity = int.Parse(Console.ReadLine());
                Console.Write("Категория (0-Электроника, 1-Одежда, 2-Продукты): ");
                var category = (Category)int.Parse(Console.ReadLine());

                manager.AddProduct(name, price, quantity, category);
                Console.WriteLine("Товар добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static void RemoveProduct(ProductManager manager)
        {
            Console.Write("Код товара: ");
            var id = Console.ReadLine();
            if (manager.RemoveProduct(id))
                Console.WriteLine("Товар удален.");
            else
                Console.WriteLine("Товар не найден.");
        }

        static void SupplyProduct(ProductManager manager)
        {
            try
            {
                Console.Write("Код товара: ");
                var id = Console.ReadLine();
                Console.Write("Количество: ");
                var amount = int.Parse(Console.ReadLine());
                manager.SupplyProduct(id, amount);
                Console.WriteLine("Поставка выполнена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void SellProduct(ProductManager manager)
        {
            try
            {
                Console.Write("Код товара: ");
                var id = Console.ReadLine();
                Console.Write("Количество: ");
                var amount = int.Parse(Console.ReadLine());
                manager.SellProduct(id, amount);
                Console.WriteLine("Продажа выполнена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void SearchProduct(ProductManager manager)
        {
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите тип поиска: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Код: ");
                    var product = manager.FindByCode(Console.ReadLine());
                    Console.WriteLine(product != null ? product.ToString() : "Товар не найден.");
                    break;
                case "2":
                    Console.Write("Название: ");
                    foreach (var p in manager.FindByName(Console.ReadLine()))
                        Console.WriteLine(p);
                    break;
                case "3":
                    Console.Write("Категория (0-Электроника, 1-Одежда, 2-Продукты): ");
                    var category = (Category)int.Parse(Console.ReadLine());
                    foreach (var p in manager.FindByCategory(category))
                        Console.WriteLine(p);
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}
    

