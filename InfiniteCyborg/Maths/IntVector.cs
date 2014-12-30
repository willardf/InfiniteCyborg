using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Maths
{
    struct IntVector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntVector(int x, int y) : this() { X = x; Y = y; }

        public float Slope { get { return Y / (float)X; } }

        public float EuLen()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public void Set(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Norm()
        {
            var len = EuLen();
            X = (int)(X / len);
            Y = (int)(Y / len);
        }

        public double Angle(IntVector input)
        {
            var x = input.X - Y;
            var y = input.Y - Y;

            double output = Math.Atan2(y, x);
            return output * 180f / Math.PI; // Output Degrees
        }

        public static float Distance(int x1, int y1, int x2, int y2)
        {
            var dx = x1 - x2;
            var dy = y1 - y2;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static IntVector operator-(IntVector a, IntVector b) { return a.Sub(b.X, b.Y); }
        public static IntVector operator +(IntVector a, IntVector b) { return new IntVector(a.X + b.X, a.Y + b.Y); }

        internal IntVector Sub(int x, int y)
        {
            return new IntVector(X - x, Y - y);
        }

        internal IntVector Add(int x, int y)
        {
            return new IntVector(X + x, Y + y);
        }
    }
}
