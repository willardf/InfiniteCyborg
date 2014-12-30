using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Genetics
{
    static class BitFieldExtensions
    {
        public static BitField RandomAnd(this BitField a, BitField b, Func<bool> randy)
        {
            BitField output = new BitField(Math.Max(a.Length, b.Length));
            output.Randomize(randy);
            return a & b & output;
        }

        public static BitField RandomOr(this BitField a, BitField b, Func<bool> randy)
        {
            BitField output = new BitField(Math.Max(a.Length, b.Length));
            output.Randomize(randy);
            return a | b | output;
        }
    }
}
