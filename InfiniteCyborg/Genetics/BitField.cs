using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Genetics
{
    public class BitField
    {
        private const int DataSize = sizeof(long) * 8;
        private long[] data;

        public BitField(int len)
        {
            Length = len;
            data = new long[len / sizeof(long) + 1];
        }

        public BitField CopyBits(BitField src, int start, int len)
        {
            for (int i = start; i < len; ++i) this[i] = src[i];
            return this;
        }

        public BitField Randomize(Func<bool> randy)
        {
            for (int i = 0; i < this.Length; ++i)
            {
                this[i] = randy();
            }

            return this;
        }

        private long Get(int bit, int nBits = 1)
        {
            int idx = bit / DataSize;
            int end = (bit + nBits) / DataSize;

            int b = bit % DataSize;
            int select = Math.Min(DataSize - b, nBits);
            long mask = (1L << select) - 1L;
            long output = ((this.data[idx] >> b) & mask);

            if (idx != end)
            {
                mask = (1L << nBits - select) - 1L;
                output |= ((this.data[end]) & mask) << select;
            }

            return output;
        }

        private void Set(int bit, int nBits = 1, long value = 0)
        {
            int idx = bit / DataSize;
            int end = (bit + nBits) / DataSize;

            var b = bit % DataSize;
            int select = Math.Min(DataSize - b, nBits);
            long mask = (1L << select) - 1L;
            this.data[idx] &= ~(mask << b);
            this.data[idx] |= (value & mask) << b;

            if (idx != end)
            {
                this.Set(bit + select, nBits - select, value >> select);
            }
        }

        public long this[int bit, int len]
        {
            get { return Get(bit, len); }
            set { Set(bit, len, value); }
        }

        public bool this[int bit]
        {
            get { return Get(bit, 1) != 0; }
            set { Set(bit, 1, (value ? 1 : 0)); }
        }

        public bool this[Bit bit]
        {
            get { return this[bit.Start]; }
            set { this[bit.Start] = value; }
        }

        public long this[BitSet b]
        {
            get { return this[b.Start, b.Length] + b.MinValue; }
            set { this[b.Start, b.Length] = value - b.MinValue; }
        }

        public static BitField operator &(BitField a, BitField b)
        {
            BitField output = new BitField(Math.Max(a.Length, b.Length));

            for (int i = 0; i < b.data.Length && i < a.data.Length; ++i)
            {
                long input = long.MaxValue;
                input &= (i < a.data.Length) ? a.data[i] : 0;
                input &= (i < b.data.Length) ? b.data[i] : 0;
                output.data[i] = input;
            }

            return output;
        }

        public static BitField operator |(BitField a, BitField b)
        {
            BitField output = new BitField(Math.Max(a.Length, b.Length));

            for (int i = 0; i < b.data.Length && i < a.data.Length; ++i)
            {
                long input = 0;
                if (i < a.data.Length) input |= a.data[i];
                if (i < b.data.Length) input |= b.data[i];
                output.data[i] = input;
            }

            return output;
        }

        public override bool Equals(object obj)
        {
            var o = obj as BitField;
            if (o != null && o.Length == this.Length)
            {
                for (int i = 0; i < o.data.Length; ++i)
                {
                    if (o.data[i] != this.data[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return data.Sum(d => d.GetHashCode());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.Length);

            for (int i = 0; i < this.Length; ++i)
            {
                sb.Insert(0, this[i] ? '1' : '0');
            }

            return sb.ToString();
        }

        public int Length { get; private set; }
    }
}