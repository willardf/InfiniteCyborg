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
        protected int focusIdx = 0;
        protected TCODConsole root;

        public List<FormComponent> Components { get; private set; }

        public Form(TCODConsole root)
        {
            this.root = root;
            Components = new List<FormComponent>();
        }

        public virtual void Draw()
        {
            Components.ForEach(c => c.Draw(root));
        }

        public virtual void HandleKey(KeyEvent key)
        {
            Components[focusIdx].HandleKey(key);
        }

        public virtual void Update(float dt)
        {
            // Nothing to do?
        }

        public virtual void Dispose()
        {
            Components.ForEach(c => c.Dispose());
        }
    }
}
