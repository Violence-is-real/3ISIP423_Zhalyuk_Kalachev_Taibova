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
