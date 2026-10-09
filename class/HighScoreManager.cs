using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace JumfrogbyMark
{
    public class HighScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }

    public static class HighScoreManager
    {
        private const int MaxEntries = 5; // เก็บคะแนนสูงสุดแค่ 5 อันดับ 
        private const char Separator = '|';
        private static readonly string FilePath = Path.Combine(Application.StartupPath, "Highscore.txt");

        public static IReadOnlyList<HighScoreEntry> Load()
        {
            // ถ้ายังไม่มีไฟล์ Highscore.txt ให้คืน list ว่าง
            if (!File.Exists(FilePath))
                return new List<HighScoreEntry>();

            var entries = new List<HighScoreEntry>();
            foreach (string line in File.ReadAllLines(FilePath))
            {
                string[] parts = line.Split(new[] { Separator }, 2);
                if (parts.Length != 2 || !int.TryParse(parts[1], out int score))
                    continue;

                entries.Add(new HighScoreEntry
                {
                    PlayerName = parts[0],
                    Score = score
                });
            }

            return SortAndLimit(entries);
        }

        public static void SaveScore(string playerName, int score)
        {
            var entries = Load().ToList();
            entries.Add(new HighScoreEntry
            {
                PlayerName = CleanName(playerName),
                Score = score
            });

            SaveEntries(SortAndLimit(entries));
        }

        private static IReadOnlyList<HighScoreEntry> SortAndLimit(IEnumerable<HighScoreEntry> entries)
        {
            // เรียงคะแนนจากมากไปน้อย
            return entries
                .OrderByDescending(entry => entry.Score)
                .ThenBy(entry => entry.PlayerName)
                .Take(MaxEntries)
                .ToList();
        }

        private static void SaveEntries(IEnumerable<HighScoreEntry> entries)
        {
            string directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllLines(FilePath, entries.Select(entry => $"{entry.PlayerName}{Separator}{entry.Score}"));
        }

        private static string CleanName(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
                return "ClassicFrog001";

            return playerName.Replace(Separator, ' ').Trim();
        }
    }
}
