using InfCy.GameCore;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Ui
{
    public abstract class FormComponent : IDisposable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int W { get; set; }
        public int H { get; set; }

        public abstract void Draw(TCODConsole root);

        public virtual void HandleKey(KeyEvent button) { }

        public virtual void Dispose() { } // Default nothing
    }
}
