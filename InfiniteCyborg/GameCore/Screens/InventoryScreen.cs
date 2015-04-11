using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public class InventoryScreen : IScreen
    {
        TCODConsole root;
        Player target;
        Camera main;
        public InventoryScreen(TCODConsole root, Player p)
        {
            this.root = root;
            this.target = p;
            this.main = new Camera(root, 40, 25) { Border = true };
        }

        public void Draw()
        {
            this.main.X = 2;
            this.main.Y = 2;
            for (int i = 0; i < target.Inventory.Count; ++i)
            {
                var item = target.Inventory[i];
                main.print(1, i + 1, "{0}", item);
            }

            main.draw();
        }

        public void HandleKey(KeyEvent key)
        {
            switch(key.button)
            {
                case Buttons.Inventory:
                    Program.PopScreen(true);
                    break;
            }
        }

        public void Update(float dt)
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
