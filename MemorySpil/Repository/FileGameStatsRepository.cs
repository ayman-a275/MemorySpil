using MemorySpil.Interface;
using MemorySpil.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace MemorySpil.Repository
{
    public class FileGameStatsRepository : IGameStatsRepository
    {

        public List<GameStat?> FindByPlayerName(string playerName)
        {
            var lines = File.ReadLines("gamestat.txt").Select(l => l.Split(','));
            List<GameStat> gamesByPlayerName = new List<GameStat>();
            foreach (var line in lines)
            {
                if (playerName == line[0])
                {
                    gamesByPlayerName.Add(new GameStat(line[0], Convert.ToInt32(line[1]), TimeSpan.Parse(line[2]) , Convert.ToDateTime(line[3])));
                }
            }

            return gamesByPlayerName;
        }

        public List<GameStat?> FindByScore(int score)
        {
            var lines = File.ReadLines("gamestat.txt").Select(l => l.Split(';'));
            List<GameStat> games = new List<GameStat>();
   
            foreach (var line in lines)
            {
                games.Add(new GameStat(line[0], Convert.ToInt32(line[1]), TimeSpan.Parse(line[2]), Convert.ToDateTime(line[3])));
            }

            return games.OrderBy(s => s.Moves).ThenBy(g => g.GameTime).ToList();
        }

        public void SaveGameStat(GameStat gameStat)
        {
            if (!File.Exists("gamestat.txt"))
            {
                File.Create("gamestat.txt");
            }

            File.AppendAllText("gamestat.txt", gameStat.ToString());
        }
    }
}
