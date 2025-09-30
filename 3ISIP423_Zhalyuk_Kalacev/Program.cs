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
        static void AddBook(Library library)
        {
            try
            {
                Console.Write("Название: ");
                var title = Console.ReadLine();
                Console.Write("Автор: ");
                var author = Console.ReadLine();
                Console.Write("Жанр (0-Fantasy, 1-SciFi, 2-Mystery, 3-Romance, 4-Horror): ");
                var genre = (Genre)Enum.Parse(typeof(Genre), Console.ReadLine());
                Console.Write("Год: ");
                var year = int.Parse(Console.ReadLine());
                Console.Write("Цена: ");
                var price = decimal.Parse(Console.ReadLine());

                library.AddBook(title, author, genre, year, price);
                Console.WriteLine("Книга добавлена");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static void RemoveBook(Library library)
        {
            Console.Write("ID книги: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (library.RemoveBook(id)) Console.WriteLine("Книга удалена");
                else Console.WriteLine("Книга не найдена");
            }
            else Console.WriteLine("Некорректный ID");
        }

        static void FindBooks(Library library)
        {
            Console.Write("Поиск по (1-названию, 2-автору, 3-жанру): ");
            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Название: ");
                    DisplayBooks(library.FindByTitle(Console.ReadLine()));
                    break;
                case "2":
                    Console.Write("Автор: ");
                    DisplayBooks(library.FindByAuthor(Console.ReadLine()));
                    break;
                case "3":
                    Console.Write("Жанр (0-4): ");
                    var genre = (Genre)int.Parse(Console.ReadLine());
                    DisplayBooks(library.FindByGenre(genre));
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }

        static void SortBooks(Library library)
        {
            Console.Write("Сортировать по (1-названию, 2-году): ");
            switch (Console.ReadLine())
            {
                case "1":
                    DisplayBooks(library.SortByTitle());
                    break;
                case "2":
                    DisplayBooks(library.SortByYear());
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }

        static void ShowPriceRange(Library library)
        {
            var (min, max) = library.GetPriceRange();
            if (min == null || max == null)
            {
                Console.WriteLine("Нет книг в библиотеке");
                return;
            }
            Console.WriteLine($"Самая дешёвая: {min.Title} ({min.Price} руб.)");
            Console.WriteLine($"Самая дорогая: {max.Title} ({max.Price} руб.)");
        }

        static void ShowAuthorStats(Library library)
        {
            foreach (var (author, count) in library.GetAuthorCounts())
                Console.WriteLine($"{author}: {count} книг(и)");
        }

        static void DisplayBooks(IEnumerable<Book> books)
        {
            foreach (var b in books)
                Console.WriteLine($"ID: {b.Id}, {b.Title} - {b.Author} ({b.Genre}), {b.Year} г., {b.Price} руб.");
        }
    }
}
