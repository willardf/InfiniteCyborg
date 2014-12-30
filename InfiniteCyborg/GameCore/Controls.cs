using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public enum Buttons { Up, Left, Right, Down, UpLeft, DownLeft, UpRight, DownRight, Lights }
    class Controls
    {
        List<Action<Buttons>> callbacks = new List<Action<Buttons>>();
        readonly static Dictionary<TCODKeyCode, Buttons> ButtonMap = new Dictionary<TCODKeyCode, Buttons>()
        {
            { TCODKeyCode.Left, Buttons.Left },
            { TCODKeyCode.Down, Buttons.Down },
            { TCODKeyCode.Right, Buttons.Right },
            { TCODKeyCode.Up, Buttons.Up },
            { TCODKeyCode.F2, Buttons.Lights }
        };

        readonly static Dictionary<char, Buttons> KeyMap = new Dictionary<char, Buttons>()
        {
            
            { 'y', Buttons.UpLeft },
            { 'b', Buttons.DownLeft },
            { 'i', Buttons.UpRight },
            { ',', Buttons.DownRight }
        };

        public void AddCallback(Action<Buttons> b)
        {
            callbacks.Add(b);
        }

        public void update(TCODKey key)
        {
            Buttons b;
            if (key.KeyCode == TCODKeyCode.Char)
            {
                if (KeyMap.TryGetValue(key.Character, out b))
                {
                    callbacks.ForEach(c => c(b));
                }
            }
            else
            {
                if (ButtonMap.TryGetValue(key.KeyCode, out b))
                {
                    callbacks.ForEach(c => c(b));
                }
            }
        }
    }
}
