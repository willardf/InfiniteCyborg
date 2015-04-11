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

        private static Stack<IScreen> Screens = new Stack<IScreen>();

        public void run()
        {
            bool running = true;
            TCODConsole.setCustomFont("celtic_garamond_10x10_gs_tc.png", (int)(TCODFontFlags.LayoutTCOD | TCODFontFlags.Greyscale));
            TCODConsole.initRoot(100, 80, "Infinite Cyborg");
            root = TCODConsole.root;

            DateTime date = new DateTime();
            Screens.Push(new NewGameScreen(root));
            Controls controls = new Controls();

            do
            {
                var topScreen = Screens.Peek();

                var frameTime = DateTime.Now - date;
                date = DateTime.Now;

                var key = TCODConsole.checkForKeypress((int)TCODKeyStatus.KeyPressed);

                if (key.KeyCode != TCODKeyCode.NoKey)
                {
                    if (key.KeyCode == TCODKeyCode.Enter && (key.LeftAlt || key.RightAlt))
                    {
                        TCODConsole.setFullscreen(!TCODConsole.isFullscreen());
                    }
                    else if (key.KeyCode == TCODKeyCode.Escape)
                    {
                        running = false;
                    }

                    controls.Update(key, topScreen.HandleKey);
                }

                topScreen.Update((float)frameTime.TotalSeconds);

                root.clear();
                foreach (var screen in Screens.Reverse())
                {
                    screen.Draw(); // TODO: Draw all in reverse order for layering
                }

                root.print(0, 0, ((int)(1 / (frameTime.TotalSeconds + .01))).ToString());

                TCODConsole.flush();
            }
            while (!TCODConsole.isWindowClosed() && running);
        }

        public static void PushScreen(IScreen s)
        {
            Screens.Push(s);
        }

        public static IScreen PopScreen(bool dispose)
        {
            var output = Screens.Pop();
            if (dispose)
            {
                output.Dispose();
            }

            return output;
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
