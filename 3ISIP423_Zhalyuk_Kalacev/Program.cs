using System;
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

    }
}