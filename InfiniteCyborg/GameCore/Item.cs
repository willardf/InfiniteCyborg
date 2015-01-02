using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    abstract class Item : IDrawable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string Name { get; set; }

        public abstract int Weight { get; set; }

        public abstract void draw(Camera root);

        public abstract void drawInfo(Camera root, int y);
    }
}
