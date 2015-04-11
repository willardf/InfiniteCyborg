using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public enum Buttons { None, Up, Left, Right, Down, UpLeft, DownLeft, UpRight, DownRight, Lights, Pickup, Inventory }

    class Controls
    {
        readonly static Dictionary<TCODKeyCode, Buttons> ButtonMap = new Dictionary<TCODKeyCode, Buttons>()
        {
            { TCODKeyCode.Left, Buttons.Left },
            { TCODKeyCode.Down, Buttons.Down },
            { TCODKeyCode.Right, Buttons.Right },
            { TCODKeyCode.Up, Buttons.Up },
            { TCODKeyCode.F2, Buttons.Lights },
            { TCODKeyCode.Tab, Buttons.Inventory }
        };

        readonly static Dictionary<char, Buttons> KeyMap = new Dictionary<char, Buttons>()
        {
            { 'p', Buttons.Pickup },
            { 'y', Buttons.UpLeft },
            { 'b', Buttons.DownLeft },
            { 'i', Buttons.UpRight },
            { ',', Buttons.DownRight }
        };

        public void Update(TCODKey key, Action<KeyEvent> callback)
        {
            Buttons b = Buttons.None;
            if (key.KeyCode == TCODKeyCode.Char)
            {
                KeyMap.TryGetValue(key.Character, out b);
                callback(new KeyEvent(key.Character, b));
            }
            else
            {
                ButtonMap.TryGetValue(key.KeyCode, out b);
                callback(new KeyEvent(key.KeyCode, b));
            }
        }
    }
}
