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

