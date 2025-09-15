using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int operationsCount;
        do
        {
            Console.Write("Введите количество операций (2-40): ");
        } while (!int.TryParse(Console.ReadLine(), out operationsCount) || operationsCount < 2 || operationsCount > 40);

        string[] names = new string[operationsCount];
        int[] amounts = new int[operationsCount];

        for (int i = 0; i < operationsCount; i++)
        {
            while (true)
            {
                Console.Write($"Введите операцию {i + 1} (формат: Название; Сумма): ");
                string input = Console.ReadLine();
                string[] parts = input.Split(';');

                if (parts.Length == 2 &&
                    int.TryParse(parts[1].Trim(), out int amount))
                {
                    names[i] = parts[0].Trim();
                    amounts[i] = amount;
                    break;
                }
                Console.WriteLine("Ошибка формата! Повторите ввод.");
            }
        }

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PrintData(names, amounts);
                    break;
                case "2":
                    ShowStatistics(amounts);
                    break;
                case "3":
                    BubbleSort(names, amounts);
                    Console.WriteLine("Данные отсортированы!");
                    break;
                case "4":
                    ConvertCurrency(amounts);
                    break;
                case "5":
                    SearchByName(names, amounts);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный пункт меню!");
                    break;
            }
        }
    }

    static void PrintData(string[] names, int[] amounts)
    {
        Console.WriteLine("\nСписок операций:");
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{names[i]}; {amounts[i]} руб.");
        }
    }

    static void ShowStatistics(int[] amounts)
    {
        int sum = 0;
        int min = amounts[0];
        int max = amounts[0];

        foreach (int amount in amounts)
        {
            sum += amount;
            if (amount < min) min = amount;
            if (amount > max) max = amount;
        }

        double average = (double)sum / amounts.Length;

        Console.WriteLine("\nСтатистика:");
        Console.WriteLine($"Сумма: {sum} руб.");
        Console.WriteLine($"Среднее: {average:F2} руб.");
        Console.WriteLine($"Минимальная сумма: {min} руб.");
        Console.WriteLine($"Максимальная сумма: {max} руб.");
    }

    static void BubbleSort(string[] names, int[] amounts)
    {
        for (int i = 0; i < amounts.Length - 1; i++)
        {
            for (int j = 0; j < amounts.Length - i - 1; j++)
            {
                if (amounts[j] > amounts[j + 1])
                {
                    (amounts[j], amounts[j + 1]) = (amounts[j + 1], amounts[j]);
                    (names[j], names[j + 1]) = (names[j + 1], names[j]);
                }
            }
        }
    }

    static void ConvertCurrency(int[] amounts)
    {
        Console.WriteLine("\nДоступные валюты:");
        Console.WriteLine("1. Доллар (USD)");
        Console.WriteLine("2. Евро (EUR)");
        Console.WriteLine("3. Юань (CNY)");
        Console.Write("Выберите валюту или введите свой курс: ");

        double rate;
        string currencySymbol;
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                rate = 95.0;
                currencySymbol = "USD";
                break;
            case "2":
                rate = 102.0;
                currencySymbol = "EUR";
                break;
            case "3":
                rate = 13.0;
                currencySymbol = "CNY";
                break;
            default:
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out rate) || rate <= 0)
                {
                    Console.WriteLine("Неверный курс!");
                    return;
                }
                currencySymbol = "иностранная валюта";
                break;
        }

        Console.WriteLine($"\nКонвертация по курсу {rate} руб./{currencySymbol}:");
        for (int i = 0; i < amounts.Length; i++)
        {
            double converted = amounts[i] / rate;
            Console.WriteLine($"{amounts[i]} руб. = {converted:F2} {currencySymbol}");
        }
    }

    static void SearchByName(string[] names, int[] amounts)
    {
        Console.Write("Введите название для поиска: ");
        string searchTerm = Console.ReadLine().ToLower();

        bool found = false;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].ToLower().Contains(searchTerm))
            {
                Console.WriteLine($"{names[i]}; {amounts[i]} руб.");
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("Операции не найдены!");
        }
    }
}