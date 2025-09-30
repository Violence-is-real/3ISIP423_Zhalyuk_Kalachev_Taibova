using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror
    }
    public class Book
    {
        private static int nextId = 1;

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book()
        {
            Id = nextId++;
        }
    }
    public class Library
    {
        private List<Book> books = new List<Book>();

        public void AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Название не может быть пустым");
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор не может быть пустым");
            if (year < 1000 || year > DateTime.Now.Year)
                throw new ArgumentException("Некорректный год издания");
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");

            books.Add(new Book
            {
                Title = title,
                Author = author,
                Genre = genre,
                Year = year,
                Price = price
            });
        }
        public bool RemoveBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return false;
            books.Remove(book);
            return true;
        }
        public IEnumerable<Book> FindByTitle(string title) =>
            books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Book> FindByAuthor(string author) =>
            books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Book> FindByGenre(Genre genre) =>
            books.Where(b => b.Genre == genre);

        public IEnumerable<Book> SortByTitle() =>
            books.OrderBy(b => b.Title);

        public IEnumerable<Book> SortByYear() =>
            books.OrderBy(b => b.Year);
        public (Book? min, Book? max) GetPriceRange()
        {
            if (!books.Any()) return (null, null);
            var min = books.OrderBy(b => b.Price).First();
            var max = books.OrderByDescending(b => b.Price).First();
            return (min, max);
        }

        public IEnumerable<(string Author, int Count)> GetAuthorCounts() =>
            books.GroupBy(b => b.Author)
                .Select(g => (g.Key, g.Count()));
    }
    class Program
    {
        static void Main(string[] args)
        {
            var library = new Library();
            library.AddTestData();

            while (true)
            {
                Console.WriteLine("\nКоманды:\n1-Добавить\n2-Удалить\n3-Найти\n4-Сортировать\n5-Ценовой диапазон\n6-Статистика авторов\n7-Выход");
                Console.Write("Введите команду: ");

                switch (Console.ReadLine())
                {
                    case "1": AddBook(library); break;
                    case "2": RemoveBook(library); break;
                    case "3": FindBooks(library); break;
                    case "4": SortBooks(library); break;
                    case "5": ShowPriceRange(library); break;
                    case "6": ShowAuthorStats(library); break;
                    case "7": return;
                    default: Console.WriteLine("Неизвестная команда"); break;
                }
            }
        }
    }
    }