using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorySpil.Model
{
    public class GameStat
    {
        public GameStat(string playerName, int moves, TimeSpan gameTime, DateTime completedAt)
        {
            PlayerName = playerName;
            Moves = moves;
            GameTime = gameTime;
            CompletedAt = completedAt;
        }

        public string PlayerName { get; set; }
        public int Moves { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime CompletedAt { get; set; }

        public override string ToString() //ændret
        {
            return $"{PlayerName},{Moves},{GameTime:mm\\:ss},{CompletedAt:yyyy-MM-dd HH:mm:ss}";
        }
    }
}