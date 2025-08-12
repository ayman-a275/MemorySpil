using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorySpil.Model
{
    public class GameStat

    {

        public string PlayerName { get; set; }

        public int Moves { get; set; }

        public TimeSpan GameTime { get; set; }

        public DateTime CompletedAt { get; set; }

    }
}
