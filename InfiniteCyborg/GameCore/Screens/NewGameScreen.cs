using InfCy.Ui;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public class NewGameScreen : Form
    {
        TextBox nameText = new TextBox();

        public NewGameScreen(TCODConsole root) : base(root)
        {
            this.nameText.X = 15;
            this.nameText.Y = 10;
            this.Components.Add(this.nameText);
            this.Components.Add(new Label("Enter Name:") { X = 2, Y = 10 });
        }

        public override void HandleKey(KeyEvent key)
        {
            if (key.tcodKey == TCODKeyCode.Enter)
            {
                GameScreen newGame = new GameScreen(root);
                newGame.Player.Name = this.nameText.Text;
                Program.PopScreen(true);
                Program.PushScreen(newGame);
            }
            else
            {
                base.HandleKey(key);
            }
        } 
    }
}
