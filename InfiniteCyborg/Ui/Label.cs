using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Ui
{
    public class Label : FormComponent
    {
        public Label(string text)
        {
            this.Text = text;
        }
        
        public override void Draw(libtcod.TCODConsole root)
        {
            root.print(this.X, this.Y, this.Text);
        } 

        public string Text { get; set; }
    }
}
