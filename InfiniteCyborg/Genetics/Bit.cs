using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Genetics
{
    class Bit
    {
        public int Start { get; private set; }

        public int End { get { return Start + 1; } }

        public Bit(int start)
        {
            this.Start = start;
        }
    }
}
