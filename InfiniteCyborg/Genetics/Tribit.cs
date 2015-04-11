using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Genetics
{
    public enum TribitState { Null = 0, True = 1, False = 2, Unknown = 3 }

    public class Tribit : BitSet
    {
        public Tribit(int start) : base(start, 2) { }
    }

    public static class TribitStateExtensions
    {
        public static bool True(this TribitState self)
        {
            return self == TribitState.True;
        }

        public static bool False(this TribitState self)
        {
            return self == TribitState.False;
        }

        public static bool Null(this TribitState self)
        {
            return self == TribitState.Null;
        }

        public static bool Unknown(this TribitState self)
        {
            return self == TribitState.Unknown;
        }
    }
}
