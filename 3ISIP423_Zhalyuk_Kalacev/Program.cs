using System;
using System.Collections.Generic;
using System.Text;

class TextStatistics
{
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; }
    public string ProcessedText { get; set; }
}

class Program
{
    // Список для хранения статистики всех обработанных текстов
    private static List<TextStatistics> allStatistics = new List<TextStatistics>();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("Введите текст (не менее 100 символов):");
            string input = Console.ReadLine();

            // Проверка длины текста
            while (input.Length < 100)
            {
                Console.WriteLine("Текст должен содержать минимум 100 символов. Попробуйте еще раз:");
                input = Console.ReadLine();
            }

            // Обработка текста и получение статистики
            TextStatistics statistics = ProcessText(input);
            allStatistics.Add(statistics);

            // Вывод текущей статистики
            PrintStatistics(statistics);

            // Запрос дальнейших действий
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Ввести новый текст");
            Console.WriteLine("2 - Показать статистику по всем текстам");
            Console.WriteLine("3 - Выйти");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    continue;
                case "2":
                    PrintAllStatistics();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Некорректный ввод");
                    break;
            }
        }
    }

    static TextStatistics ProcessText(string text)
    {
        TextStatistics stats = new TextStatistics
        {
            ProcessedText = text,
            LetterFrequency = new Dictionary<char, int>()
        };

        // Подсчет предложений (по знакам препинания)
        char[] sentenceSeparators = { '.', '!', '?' };
        foreach (char c in text)
        {
            if (Array.IndexOf(sentenceSeparators, c) >= 0)
            {
                stats.SentenceCount++;
            }
        }

        // Разделение текста на слова
        string[] words = SplitIntoWords(text);
        stats.WordCount = words.Length;

        // Поиск самого короткого и длинного слова
        if (words.Length > 0)
        {
            stats.ShortestWord = words[0];
            stats.LongestWord = words[0];
            foreach (string word in words)
            {
                if (word.Length < stats.ShortestWord.Length)
                    stats.ShortestWord = word;
                if (word.Length > stats.LongestWord.Length)
                    stats.LongestWord = word;
            }
        }

        // Подсчет гласных и согласных
        char[] vowels = { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };
        foreach (char c in text.ToLower())
        {
            if (char.IsLetter(c))
            {
                // Обновление частоты букв
                if (!stats.LetterFrequency.ContainsKey(c))
                    stats.LetterFrequency[c] = 0;
                stats.LetterFrequency[c]++;

                // Проверка на гласную/согласную
                bool isVowel = false;
                foreach (char v in vowels)
                {
                    if (c == v)
                    {
                        isVowel = true;
                        break;
                    }
                }

                if (isVowel) stats.VowelCount++;
                else stats.ConsonantCount++;
            }
        }

        return stats;
    }

    // Метод для разделения текста на слова без использования LINQ
    static string[] SplitIntoWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder currentWord = new StringBuilder();

        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c))
            {
                currentWord.Append(c);
            }
            else
            {
                if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }
        }

        // Добавление последнего слова, если оно есть
        if (currentWord.Length > 0)
            words.Add(currentWord.ToString());

        return words.ToArray();
    }

    static void PrintStatistics(TextStatistics stats)
    {
        Console.WriteLine("\n=== СТАТИСТИКА ТЕКСТА ===");
        Console.WriteLine($"Количество слов: {stats.WordCount}");
        Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
        Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
        Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
        Console.WriteLine($"Гласные буквы: {stats.VowelCount}");
        Console.WriteLine($"Согласные буквы: {stats.ConsonantCount}");

        Console.WriteLine("Частота букв:");
        foreach (var entry in stats.LetterFrequency)
        {
            Console.WriteLine($"'{entry.Key}': {entry.Value}");
        }
    }

    static void PrintAllStatistics()
    {
        Console.WriteLine("\n=== СТАТИСТИКА ПО ВСЕМ ТЕКСТАМ ===");
        for (int i = 0; i < allStatistics.Count; i++)
        {
            Console.WriteLine($"\n--- Текст #{i + 1} ---");
            PrintStatistics(allStatistics[i]);
            Console.WriteLine($"Первые 100 символов: {allStatistics[i].ProcessedText.Substring(0, Math.Min(100, allStatistics[i].ProcessedText.Length))}...");
        }
    }
}