using InfCy.GameCore;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Ui
{
    public class Form : IScreen
    {
        private int focusIdx = 0;
        public List<FormComponent> Components { get; private set; }
        TCODConsole root;

        public Form(TCODConsole root)
        {
            this.root = root;
            Components = new List<FormComponent>();
        }

        public void Draw()
        {
            Components.ForEach(c => c.Draw(root));
        }

        public void HandleKey(Buttons key)
        {
            Components[focusIdx].HandleKey(key);
        }

        public void Update()
        {
            // Nothing to do?
        }

        public void Dispose()
        {
            // Nothing to do.
        }
    }
}
