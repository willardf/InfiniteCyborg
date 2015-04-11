using InfCy.GameCore;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Ui
{
    public class TextBox : Label
    {
        public TextBox()
            : base("")
        {
        }

        public override void HandleKey(KeyEvent button)
        {
            char c = button.keyChar;
            if (Char.IsLetterOrDigit(c))
            {
                this.Text += c;
            }
            else if (button.tcodKey == TCODKeyCode.Backspace && this.Text.Length > 0)
            {
                this.Text = this.Text.Substring(0, this.Text.Length - 1);
            }
            else
            {
                // Failed to input
            }
        }
    }
}
