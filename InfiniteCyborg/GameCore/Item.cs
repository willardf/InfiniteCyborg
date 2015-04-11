using InfCy.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public abstract class Item : IDrawable
    {
        protected BitField data = new BitField(1);

        public Item(BitField data)
        {
            this.data = data;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public virtual string Name { get; set; }

        public abstract int Weight { get; set; }

        public abstract void Draw(Camera root);

        public abstract void DrawInfo(Camera root, int y);
    }
}
