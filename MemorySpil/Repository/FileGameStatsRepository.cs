using MemorySpil.Interface;
using MemorySpil.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorySpil.Repository
{
    public class FileGameStatsRepository : IGameStatsRepository
    {
        private readonly string _filePath = "gamestat.csv";

        public List<GameStat?> FindByPlayerName(string playerName)
        {
            List<GameStat?> gamesByPlayerName = new List<GameStat?>();

            try
            {
                if (!File.Exists(_filePath))
                    return gamesByPlayerName;

                var lines = File.ReadAllLines(_filePath);

                // Skip header if exists
                var dataLines = lines.Skip(lines.Length > 0 && lines[0].Contains("PlayerName") ? 1 : 0);

                foreach (var line in dataLines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 4 && parts[0].Equals(playerName, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            gamesByPlayerName.Add(new GameStat(
                                parts[0],
                                int.Parse(parts[1]),
                                TimeSpan.Parse(parts[2]),
                                DateTime.Parse(parts[3])
                            ));
                        }
                        catch (Exception ex)
                        {
                            // Log parsing error and continue
                            Console.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return gamesByPlayerName;
        }

        public List<GameStat?> FindByScore(int score)
        {
            List<GameStat?> games = new List<GameStat?>();

            try
            {
                if (!File.Exists(_filePath))
                    return games;

                var lines = File.ReadAllLines(_filePath);

                // Skip header if exists
                var dataLines = lines.Skip(lines.Length > 0 && lines[0].Contains("PlayerName") ? 1 : 0);

                foreach (var line in dataLines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 4)
                    {
                        try
                        {
                            games.Add(new GameStat(
                                parts[0],
                                int.Parse(parts[1]),
                                TimeSpan.Parse(parts[2]),
                                DateTime.Parse(parts[3])
                            ));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing line: {line}. Error: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            // Return top 10 sorted by moves (ascending), then by time (ascending)
            return games.Where(g => g != null)
                       .OrderBy(s => s.Moves)
                       .ThenBy(g => g.GameTime)
                       .Take(10)
                       .ToList();
        }

        public void SaveGameStat(GameStat gameStat)
        {
            try
            {
                bool fileExists = File.Exists(_filePath);

                // Create header if file doesn't exist
                if (!fileExists)
                {
                    File.WriteAllText(_filePath, "PlayerName,Moves,GameTime,CompletedAt\n");
                }

                // Append the game stat
                File.AppendAllText(_filePath, gameStat.ToString() + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game stat: {ex.Message}");
                throw; // Re-throw to let the caller handle it
            }
        }
    }
}