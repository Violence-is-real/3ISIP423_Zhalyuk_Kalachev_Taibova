using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
    private static HashSet<string> conjunctions = new HashSet<string>
{
"и", "а", "но", "да", "или", "либо", "что", "чтобы", "как", "когда",
"пока", "если", "хотя", "потому", "поэтому", "зато", "не", "ни"
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
    static string[] SplitTextIntoWords(string text)
    {
        // Разделители для слов: пробелы, знаки препинания
        char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '"', '(', ')', '[', ']', '{', '}', '\t', '\n', '\r' };
        return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
    }

    static int CountWordsWithoutConjunctionsAndNumbers(string[] words)
    {
        int count = 0;

        foreach (string word in words)
        {
            string cleanWord = CleanWord(word);

            // Пропускаем пустые слова
            if (string.IsNullOrEmpty(cleanWord)) continue;

            // Пропускаем союзы
            if (conjunctions.Contains(cleanWord.ToLower())) continue;

            // Пропускаем числа
            if (IsNumber(cleanWord)) continue;

            count++;
        }

        return count;
    }
    static string CleanWord(string word)
    {
        // Удаляем знаки препинания с начала и конца слова
        int start = 0;
        int end = word.Length - 1;

        while (start <= end && char.IsPunctuation(word[start])) start++;
        while (end >= start && char.IsPunctuation(word[end])) end--;

        if (start > end) return string.Empty;

        return word.Substring(start, end - start + 1);
    }
    static bool IsNumber(string word)
    {
        
        if (string.IsNullOrEmpty(word)) return false;

        bool hasDigit = false;
        bool hasDecimalSeparator = false;

        foreach (char c in word)
        {
            if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (c == '.' || c == ',')
            {
                if (hasDecimalSeparator) return false;
                hasDecimalSeparator = true;
            }
            else
            {
                return false;
            }
        }

        return hasDigit;
    }
    static void FindShortestAndLongestWords(string[] words, TextStatistics stats)
    {
        string shortest = null;
        string longest = null;

        foreach (string word in words)
        {
            string cleanWord = CleanWord(word);

            // Пропускаем пустые слова, союзы и числа
            if (string.IsNullOrEmpty(cleanWord)) continue;
            if (conjunctions.Contains(cleanWord.ToLower())) continue;
            if (IsNumber(cleanWord)) continue;

            if (shortest == null || cleanWord.Length < shortest.Length)
            {
                shortest = cleanWord;
            }

            if (longest == null || cleanWord.Length > longest.Length)
            {
                longest = cleanWord;
            }
        }

        stats.ShortestWord = shortest ?? "не найдено";
        stats.LongestWord = longest ?? "не найдено";
    }
    static int CountSentences(string text)
    {
        int count = 0;
        bool inSentence = false;

        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c))
            {
                if (!inSentence)
                {
                    count++;
                    inSentence = true;
                }
            }
            else if (c == '.' || c == '!' || c == '?' || c == '\n')
            {
                inSentence = false;
            }
        }

        return count;
    }
    static void CountVowelsAndConsonants(string text, TextStatistics stats)
    {
        int vowelCount = 0;
        int consonantCount = 0;

        foreach (char c in text.ToLower())
        {
            if (vowels.Contains(c))
            {
                vowelCount++;
            }
            else if (consonants.Contains(c))
            {
                consonantCount++;
            }
        }

        stats.VowelCount = vowelCount;
        stats.ConsonantCount = consonantCount;
    }
    static Dictionary<char, int> CalculateLetterFrequency(string text)
    {
        Dictionary<char, int> frequency = new Dictionary<char, int>();

        foreach (char c in text.ToLower())
        {
            if (char.IsLetter(c))
            {
                if (frequency.ContainsKey(c))
                {
                    frequency[c]++;
                }
                else
                {
                    frequency[c] = 1;
                }
            }
        }

        return frequency;
    }
    static void ProcessLetterRemoval(TextStatistics originalStats)
    {
        Console.Write("\nХотите удалить определенные буквы из текста? (y/n): ");
        string response = Console.ReadLine()?.ToLower();

        if (response != "y" && response != "yes" && response != "да") return;

        Console.Write("Введите буквы для удаления (без пробелов): ");
        string lettersToRemove = Console.ReadLine()?.ToLower();

        if (string.IsNullOrEmpty(lettersToRemove))
        {
            Console.WriteLine("Не введены буквы для удаления.");
            return;
        }

        // Удаляем буквы из текста
        string modifiedText = RemoveLetters(originalStats.Text, lettersToRemove);

        // Создаем новую статистику для модифицированного текста
        TextStatistics modifiedStats = new TextStatistics
        {
            Text = modifiedText,
            ProcessedAt = DateTime.Now
        };

        // Пересчитываем статистику
        CalculateBasicStatistics(modifiedStats);

        // Показываем результаты
        ShowStatistics(modifiedStats, $"СТАТИСТИКА ПОСЛЕ УДАЛЕНИЯ БУКВ: {lettersToRemove}");

        // Сохраняем модифицированную статистику
        allStatistics.Add(modifiedStats);
    }
    static string RemoveLetters(string text, string lettersToRemove)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            char lowerC = char.ToLower(c);
            if (!lettersToRemove.Contains(lowerC.ToString()))
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
    static void ShowStatistics(TextStatistics stats, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        Console.WriteLine($"Текст: {stats.Text.Substring(0, Math.Min(100, stats.Text.Length))}...");
        Console.WriteLine($"Обработан: {stats.ProcessedAt}");
        Console.WriteLine($"Количество слов (без союзов и чисел): {stats.WordCount}");
        Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
        Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
        Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
        Console.WriteLine($"Гласных букв: {stats.VowelCount}");
        Console.WriteLine($"Согласных букв: {stats.ConsonantCount}");

        Console.WriteLine("Частота встречаемости букв:");
        foreach (var pair in stats.LetterFrequency)
        {
            Console.WriteLine($" {pair.Key}: {pair.Value}");
        }
    }

    static void ShowPastStatistics()
    {
        if (allStatistics.Count == 0)
        {
            Console.WriteLine("Статистика по прошлым текстам отсутствует.");
            return;
        }

        Console.WriteLine($"\n=== СТАТИСТИКА ПО ПРОШЛЫМ ТЕКСТАМ (всего: {allStatistics.Count}) ===");

        for (int i = 0; i < allStatistics.Count; i++)
        {
            Console.WriteLine($"\n--- Текст #{i + 1} ---");
            Console.WriteLine($"Обработан: {allStatistics[i].ProcessedAt}");
            Console.WriteLine($"Количество слов: {allStatistics[i].WordCount}");
            Console.WriteLine($"Предложений: {allStatistics[i].SentenceCount}");
            Console.WriteLine($"Гласных/согласных: {allStatistics[i].VowelCount}/{allStatistics[i].ConsonantCount}");

            if (allStatistics[i].LetterFrequency.Count > 0)
            {
                var firstLetter = allStatistics[i].LetterFrequency.Keys.GetEnumerator().Current;
                Console.WriteLine($"Букв в частотном анализе: {allStatistics[i].LetterFrequency.Count}");
            }
        }

        Console.Write("\nПоказать детальную статистику для конкретного текста? (y/n): ");
        string response = Console.ReadLine()?.ToLower();

        if (response == "y" || response == "yes" || response == "да")
        {
            Console.Write($"Введите номер текста (1-{allStatistics.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= allStatistics.Count)
            {
                ShowStatistics(allStatistics[index - 1], $"ДЕТАЛЬНАЯ СТАТИСТИКА ТЕКСТА #{index}");
            }
            else
            {
                Console.WriteLine("Неверный номер текста.");
            }
        }
    }
}



