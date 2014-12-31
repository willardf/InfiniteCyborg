using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Genetics
{
    public class BitSet
    {
        public BitSet(int start, int len, bool signed = false)
        {
            this.Start = start;
            this.Length = len;
            this.Signed = signed;
        }
        public int Start { get; private set; }

        public int Length { get; private set; }

        public int End { get { return Start + Length; } }

        public bool Signed { get; private set; }

        public long MaxValue { get { return Signed ? 1 << (Length - 1) : (1 << Length) - 1; } }

        public long MinValue { get { return Signed ? -(1 << (Length - 1)) : 0; } }
    }
}
