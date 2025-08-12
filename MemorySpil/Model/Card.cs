using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemorySpil.Model
{
    public class Card

    {
        public int Id { get; set; }

        public string Symbol { get; set; } // "", "", "" osv. Eller bogstaver

        public bool IsFlipped { get; set; }

        public bool IsMatched { get; set; }

    }
}
