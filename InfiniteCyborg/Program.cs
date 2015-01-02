using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;
using InfCy.GameCore;

namespace InfCy
{
    class Program : IDisposable
    {
        public Program() { }

        TCODConsole root;

        Stack<IScreen> Screens = new Stack<IScreen>();

        public void run()
        {
            bool running = true;
            TCODConsole.setCustomFont("celtic_garamond_10x10_gs_tc.png", (int)(TCODFontFlags.LayoutTCOD | TCODFontFlags.Greyscale));
            TCODConsole.initRoot(100, 80, "Infinite Cyborg");
            root = TCODConsole.root;

            DateTime date = new DateTime();
            using (Game game = new Game(root))
            {
                Screens.Push(game);
                Controls controls = new Controls();

                do
                {
                    var topScreen = Screens.Peek();

                    var frameTime = DateTime.Now - date;
                    var key = TCODConsole.checkForKeypress((int)TCODKeyStatus.KeyPressed);

                    if (key.KeyCode != TCODKeyCode.NoKey)
                    {
                        if (key.KeyCode == TCODKeyCode.Enter && (key.LeftAlt || key.RightAlt))
                        {
                            // ALT-ENTER : switch fullscreen
                            TCODConsole.setFullscreen(!TCODConsole.isFullscreen());
                        }
                        else if (key.KeyCode == TCODKeyCode.Escape)
                        {
                            running = false;
                        }

                        controls.update(key, topScreen.handleKey);
                    }

                    
                    topScreen.update(); // TODO: Timestamp

                    root.clear();
                    topScreen.draw();
                    root.print(0, 0, ((int)(1 / (frameTime.TotalSeconds + .01))).ToString());

                    TCODConsole.flush();
                    date = DateTime.Now;
                }
                while (!TCODConsole.isWindowClosed() && running);
            }
        }

        public void Dispose()
        {
            root.Dispose();
        }

        static void Main(string[] args)
        {
            using (Program p = new Program())
            {
                p.run();
            }
        }
    }
}
