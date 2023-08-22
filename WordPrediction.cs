using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class WordPrediction
{
    static Dictionary<string, Dictionary<string, int>> wordPairs = new Dictionary<string, Dictionary<string, int>>();
    static Random random = new Random();

    public static string previousWord = "";
    public static string currentWord = "";

    // Predicts the next word based on the provided word
    public string PredictNextWord(string word)
    {
        string ret = "";
        LoadWordPairsFromCSV("wordPairs.csv");

        currentWord = word;

        if (!string.IsNullOrWhiteSpace(previousWord) && !string.IsNullOrEmpty(currentWord))
        {
            if (!wordPairs.ContainsKey(previousWord))
            {
                wordPairs[previousWord] = new Dictionary<string, int>();
            }

            if (!wordPairs[previousWord].ContainsKey(currentWord))
            {
                wordPairs[previousWord][currentWord] = 0;
            }

            wordPairs[previousWord][currentWord]++;
        }

        if (!string.IsNullOrEmpty(currentWord))
        {
            string wordCopy = currentWord;
            ret = GenerateAndDisplayNextWord(currentWord);
        }

        previousWord = currentWord;

        SaveWordPairsToCSV("wordPairs.csv");
        return ret;
    }

    // Generates and returns the most likely next word based on the current word
    public static string GenerateAndDisplayNextWord(string currentWord)
    {
        string mostLikelyNextWord;
        if (wordPairs.ContainsKey(currentWord))
        {
            var nextWords = wordPairs[currentWord];
            int totalOccurrences = nextWords.Values.Sum();

            mostLikelyNextWord = FindMostLikelyNextWord(nextWords, totalOccurrences);
            return mostLikelyNextWord;
        }
        else
        {
            return "";
        }
    }

    // Finds the most likely next word based on occurrence probabilities
    static string FindMostLikelyNextWord(Dictionary<string, int> nextWords, int totalOccurrences)
    {
        List<string> possibleWords = new List<string>();
        List<double> probabilities = new List<double>();

        foreach (var pair in nextWords)
        {
            double probability = (double)pair.Value / totalOccurrences;
            possibleWords.Add(pair.Key);
            probabilities.Add(probability);
        }

        double randomValue = random.NextDouble();
        double cumulativeProbability = 0;

        for (int i = 0; i < possibleWords.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return possibleWords[i];
            }
        }

        return possibleWords[random.Next(possibleWords.Count)];
    }

    // Loads word pairs from a CSV file
    static void LoadWordPairsFromCSV(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 3 && int.TryParse(parts[2], out int count))
                {
                    if (!wordPairs.ContainsKey(parts[0]))
                    {
                        wordPairs[parts[0]] = new Dictionary<string, int>();
                    }

                    wordPairs[parts[0]][parts[1]] = count;
                }
            }
        }
    }

    // Saves word pairs to a CSV file
    static void SaveWordPairsToCSV(string filePath)
    {
        List<string> lines = new List<string>();

        foreach (var entry in wordPairs)
        {
            foreach (var subEntry in entry.Value)
            {
                lines.Add($"{entry.Key},{subEntry.Key},{subEntry.Value}");
            }
        }

        File.WriteAllLines(filePath, lines);
    }

    // Checks if the input is a partial match to a word
    static bool IsPartialMatch(string input, string word, double threshold)
    {
        int maxDistance = (int)(word.Length * (1 - threshold));
        int distance = ComputeLevenshteinDistance(input, word);

        return distance <= maxDistance;
    }

    // Computes the Levenshtein distance between two strings
    static int ComputeLevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        for (int i = 0; i <= m; i++)
            d[i, 0] = i;

        for (int j = 0; j <= n; j++)
            d[0, j] = j;

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                if (s[i - 1] == t[j - 1])
                    d[i, j] = d[i - 1, j - 1];
                else
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + 1);
            }
        }

        return d[m, n];
    }
}
