using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public class KeyEvent
    {
        public TCODKeyCode tcodKey;
        public char keyChar;
        public Buttons button;

        public KeyEvent(char p, Buttons b)
        {
            this.keyChar = p;
            this.button = b;
        }

        public KeyEvent(TCODKeyCode p, Buttons b)
        {
            this.tcodKey = p;
            this.button = b;
        }
    }
}
