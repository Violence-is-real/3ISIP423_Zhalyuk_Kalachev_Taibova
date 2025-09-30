using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;

class TextStatistics
{
    public string Text { get; set; }
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; }
    public DateTime ProcessedAt { get; set; }
}

class Program
{
    // Список для хранения статистики по всем текстам
    private static List<TextStatistics> allStatistics = new List<TextStatistics>();

    // Множества гласных и согласных букв (русский и английский алфавиты)
    private static HashSet<char> vowels = new HashSet<char>
{
'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
'a', 'e', 'i', 'o', 'u', 'y'
};

    private static HashSet<char> consonants = new HashSet<char>
{
'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z'
};

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\nАнализатор текста");
            Console.WriteLine("1. Анализ нового текста");
            Console.WriteLine("2. Просмотр статистики по прошлым текстам");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AnalyzeNewText();
                    break;
                case "2":
                    ShowPastStatistics();
                    break;
                case "3":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }
    static void AnalyzeNewText()
    {
        string text = GetTextFromUser();
        if (string.IsNullOrEmpty(text)) return;

        // Создаем объект для статистики
        TextStatistics stats = new TextStatistics
        {
            Text = text,
            ProcessedAt = DateTime.Now
        };

        // Вычисляем базовую статистику
        CalculateBasicStatistics(stats);

        // Показываем результаты
        ShowStatistics(stats, "БАЗОВАЯ СТАТИСТИКА");

        // Предлагаем удалить буквы и пересчитать
        ProcessLetterRemoval(stats);

        // Сохраняем статистику
        allStatistics.Add(stats);

        Console.WriteLine("\nСтатистика сохранена!");
    }
    static string GetTextFromUser()
    {
        string text;
        while (true)
        {
            Console.WriteLine("\nВведите текст (минимум 100 символов):");
            Console.WriteLine("(для выхода введите 'exit')");
            text = Console.ReadLine();

            if (text?.ToLower() == "exit") return null;

            if (string.IsNullOrEmpty(text) || text.Length < 100)
            {
                Console.WriteLine($"Текст должен содержать минимум 100 символов! Сейчас: {text?.Length ?? 0}");
                continue;
            }

            break;
        }
        return text;
    }
    static void CalculateBasicStatistics(TextStatistics stats)
    {
        string text = stats.Text;

        // Подсчет слов (без учета союзов и чисел)
        string[] words = SplitTextIntoWords(text);
        stats.WordCount = CountWordsWithoutConjunctionsAndNumbers(words);

        // Поиск самого короткого и длинного слова
        FindShortestAndLongestWords(words, stats);

        // Подсчет предложений
        stats.SentenceCount = CountSentences(text);

        // Подсчет гласных и согласных
        CountVowelsAndConsonants(text, stats);

        // Статистика по буквам
        stats.LetterFrequency = CalculateLetterFrequency(text);
    }
}


