using InfCy.GameCore;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Ui
{
    public abstract class FormComponent
    {
        public int X { get; set; }
        public int Y { get; set; }

        public abstract void draw(TCODConsole root);

        public abstract void handleKey(Buttons button);
    }
}
