using InfCy.Maths;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public class Enemy : Mover
    {
        public Weapon weapon = Weapon.Fists;

        public Enemy()
        {
            Demeanor = Hostile.Enemy;
        }

        public override bool DoTurn()
        {
            var randy = TCODRandom.getInstance();
            var dx = randy.getInt(-1, 1);
            var dy = randy.getInt(-1, 1);
            if (!Battle.ResolveAttack(this, this.weapon, new IntVector(X + dx, Y + dy), GameScreen.CurrentGame))
            {
                Walk(dx, dy);
            }

            return true;
        }

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, Name[0]);
        }

        public override void DrawInfo(Camera root, int y)
        {
            root.print(1, y, "{0}: {1}/{2}", Name, Health, MaxHealth);
        }

        public override void OnDeath(Mover killer)
        {
            GameScreen.CurrentGame.RemoveMover(this);
        }

        protected override void OnMove()
        {
            
        }

        public override int Speed
        {
            get { return 1; }
            set { }
        }
    }
}
