using MemorySpil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MemorySpil.Interface
{
    interface IGameStatsRepository
    {
        //public List<GameStat> GetAllGameStats();
        public List<GameStat?> FindByPlayerName(string playerName);
        public List<GameStat?> FindByScore(int score);
        public void SaveGameStat(GameStat gameStat);
    }
}
