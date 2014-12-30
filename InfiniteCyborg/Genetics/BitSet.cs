using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Genetics
{
    class BitSet
    {
        public int Start { get; private set; }

        public int Length { get; private set; }
        public int End { get { return Start + Length; } }
        public bool Signed { get; private set; }

        public BitSet(int start, int len, bool signed = false)
        {
            this.Start = start;
            this.Length = len;
            this.Signed = signed;
        }
    }
}
